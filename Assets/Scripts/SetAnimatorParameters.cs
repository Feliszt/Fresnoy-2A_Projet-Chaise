using UnityEngine;

public class SetAnimatorParameters : MonoBehaviour
{
    private Animator animator;
    public ControlTimeline ctrlTimeline;

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

    public void SetPauseTrue()
    {
        animator.SetBool("Pause", true);
    }

    public void SetPlayTrue()
    {
        animator.SetBool("Play", true);
    }
    
    public void TogglePlayPause()
    {
        if (animator.GetBool("Pause"))
        {
            animator.SetBool("Play", true);
            animator.SetBool("Pause", false);
            ctrlTimeline.ResumeTimeline();
        }

        if (animator.GetBool("Play"))
        {
            animator.SetBool("Play", false);
            animator.SetBool("Pause", true);
            ctrlTimeline.PauseTimeline();
        }
    }
}
