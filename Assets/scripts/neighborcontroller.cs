using UnityEngine;

public class NeighbourController : MonoBehaviour
{
    private Animator animator;

    [Header("Random Timing")]
    public float minDecisionTime = 2f;
    public float maxDecisionTime = 6f;

    [Header("Animator State Names")]
    public string wakeUpStateName = "main_rig|wake_up";

    private float nextDecisionTime;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("NeighbourController: No Animator found on this GameObject.");
            enabled = false;
            return;
        }

        ScheduleNextDecision();
    }

    void Update()
    {
        if (Time.time >= nextDecisionTime)
        {
            MakeDecision();
            ScheduleNextDecision();
        }
    }

    void MakeDecision()
    {
        int choice = Random.Range(0, 2);

        if (choice == 0)
        {
            animator.SetTrigger("Smoke");
        }
        else
        {
            animator.SetTrigger("WakeUp");
        }
    }

    void ScheduleNextDecision()
    {
        nextDecisionTime = Time.time + Random.Range(minDecisionTime, maxDecisionTime);
    }

    public bool IsAlert()
    {
        if (animator == null)
            return false;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        return state.IsName(wakeUpStateName);
    }
}