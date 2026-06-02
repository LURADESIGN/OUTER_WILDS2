using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController controller;

    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float turnSpeed = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    [Header("Animation")]
    public Animator animator;

    Vector3 velocity;
    bool isGrounded;

    Vector3 airMoveDirection;

    void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (controller == null)
        {
            Debug.LogError("PlayerMovement: No CharacterController found on this GameObject.");
        }

        if (animator == null)
        {
            Debug.LogWarning("PlayerMovement: No Animator found. Movement still works, but animations will not play.");
        }
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

        // Prevent faster diagonal movement
        inputMove = Vector3.ClampMagnitude(inputMove, 1f);

        // Ground movement
        if (isGrounded)
        {
            // Rotate only while moving
            if (inputMove.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputMove);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    turnSpeed * Time.deltaTime
                );
            }

            // Move on ground
            controller.Move(inputMove * speed * Time.deltaTime);

            // Store air momentum
            airMoveDirection = inputMove * speed;
        }
        else
        {
            // In air: keep stored momentum
            controller.Move(airMoveDirection * Time.deltaTime);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animation update
        UpdateAnimations(inputMove);
    }

    void UpdateAnimations(Vector3 inputMove)
    {
        if (animator == null)
            return;

        float movementAmount = inputMove.magnitude;

        animator.SetFloat("Speed", movementAmount);
        animator.SetBool("Grounded", isGrounded);
    }
}