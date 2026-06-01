using UnityEngine;

public class NeighbourController : MonoBehaviour
{
    private Animator animator;

    [Header("Timing")]
    public float minWakeTime = 3f;
    public float maxWakeTime = 10f;
    public bool IsAware = false;

    private float nextWakeTime;

    void Start()
    {
        animator = GetComponent<Animator>();

        ScheduleNextWake();
    }

    void Update()
    {
        if (Time.time >= nextWakeTime)
        {
            animator.SetTrigger("WakeUp");
            IsAware = true;

            ScheduleNextWake();
        }
    }

    void ScheduleNextWake()
    {
        nextWakeTime = Time.time + Random.Range(minWakeTime, maxWakeTime);
    }
}