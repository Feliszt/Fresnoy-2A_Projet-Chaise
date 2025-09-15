using UnityEngine;

public class GoToInit : StateMachineBehaviour
{
    public string MoverObjectName;
    public string OSCWithMasterName;
    private GoToPose goToPose;
    private OscWithMaster OSCWithMaster;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //
        animator.SetBool("TimelineFinished", false);

        // get object dynamically
        GameObject mover = GameObject.Find(MoverObjectName);
        GameObject OSCWithMasterObj = GameObject.Find(OSCWithMasterName);
        if (mover != null && OSCWithMasterObj != null)
        {
            goToPose = mover.GetComponent<GoToPose>();
            OSCWithMaster = OSCWithMasterObj.GetComponent<OscWithMaster>();
            goToPose.setGoToInit();
            OSCWithMaster.SendReadyStatus();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
