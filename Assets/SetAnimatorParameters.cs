using UnityEngine;

public class SetAnimatorParameters : MonoBehaviour
{
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetStopTrue()
    {
        animator.SetBool("Stop", true);
    }
}
