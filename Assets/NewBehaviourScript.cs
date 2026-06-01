
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float turnSpeed = 10f;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    Vector3 airMoveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 inputMove = forward * z + right * x;

        // GROND BEWEGING
        if (isGrounded)
        {
            // Rotatie alleen als je beweegt
            if (inputMove.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputMove);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }

            // Beweging op de grond
            controller.Move(inputMove * speed * Time.deltaTime);

            // HIER slaan we momentum op (belangrijk!)
            airMoveDirection = inputMove * speed;
        }
        else
        {
            // In de lucht: gebruik opgeslagen momentum
            controller.Move(airMoveDirection * Time.deltaTime);
        }

        // SPRINGEN
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Zwaartekracht
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}