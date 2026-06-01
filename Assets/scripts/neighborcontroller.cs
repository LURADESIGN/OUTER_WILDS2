using UnityEngine;

public class neighborcontroller : MonoBehaviour
{
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()

    {
        animator = GetComponent<Animator>();   
    }

    public void WakeNeighbor()
  {
        animator.SetTrigger("WakeUp");
    }

    public void Smoke()
    {
        animator.SetTrigger("Smoke");
    }
  }

