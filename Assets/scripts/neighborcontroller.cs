using UnityEngine;

public class NeighbourController : MonoBehaviour
{
    private Animator animator;

    [Header("Waking Up Detection Window")]
    [SerializeField] private float safeTimeStart = 0.5f;
    [SerializeField] private float safeTimeEnd = 0.58f;

    [Header("Random Timing")]
    public float minDecisionTime = 2f;
    public float maxDecisionTime = 6f;

    [Header("Animator State Names")]
    public string idleStateName = "main_rig|sleeping";
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
        if (Time.time >= nextDecisionTime && IsIdle())
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

    private bool IsIdle()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        return state.IsName(idleStateName);
    }

    public bool IsAlert()
    {
        if (animator == null)
            return false;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (!state.IsName(wakeUpStateName))
            return false;

        float animationLength = state.length;

        float normalizedTime = state.normalizedTime % 1f;
        float currentTime = normalizedTime * animationLength;
        float timeRemaining = animationLength - currentTime;

        bool afterSafeStart = currentTime > safeTimeStart;
        bool beforeSafeEnd = timeRemaining > safeTimeEnd;

        return afterSafeStart && beforeSafeEnd;
    }
}