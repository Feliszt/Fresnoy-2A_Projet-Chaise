using extOSC;
using UnityEngine;
using UnityEngine.Playables;

public class PlayTimeline : StateMachineBehaviour
{
    public string TimelineName;
    public string MoverObjectName;
    public string OSCWithMasterName;

    private ControlTimeline controlTimeline;
    private FollowPose followPose;
    private OscWithMaster OSCWithMaster;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // get object dynamically
        GameObject timeline = GameObject.Find(TimelineName);
        GameObject mover = GameObject.Find(MoverObjectName);

        if (mover != null && timeline != null)
        {
            controlTimeline = timeline.GetComponent<ControlTimeline>();
            followPose = mover.GetComponent<FollowPose>();

            Debug.Log("[SM -> Timeline]\tplay.");
            controlTimeline.PlayTimeline();
            followPose.enabled = true;
            animator.SetBool("FollowPoseEnabled", true);
            animator.SetBool("AtFirst", false);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        if (animator.GetBool("Play") && animator.GetBool("Pause"))
        {
            controlTimeline.ResumeTimeline();
            animator.SetBool("Pause", false);
        }    
        */
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        followPose.enabled = false;
        animator.SetBool("FollowPoseEnabled", false);
        animator.SetBool("Play", false);
        animator.SetBool("Stop", false);
        controlTimeline.StopTimeline();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
