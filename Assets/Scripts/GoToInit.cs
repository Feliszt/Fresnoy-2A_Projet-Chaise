using UnityEngine;

public class GoToInit : StateMachineBehaviour
{
    public string MoverObjectName;
    public string OSCWithMasterName;
    public string OSCWithChataigneName;
    private GoToPose goToPose;
    private OscWithMaster OSCWithMaster;
    private OSCSendTime OSCWithChataigne;    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //
        animator.SetBool("TimelineFinished", false);

        // get object dynamically
        GameObject mover = GameObject.Find(MoverObjectName);
        GameObject OSCWithMasterObj = GameObject.Find(OSCWithMasterName);
        GameObject OSCWithChataigneObj = GameObject.Find(OSCWithChataigneName);
        if (mover != null && OSCWithMasterObj != null && OSCWithChataigneObj != null)
        {
            goToPose = mover.GetComponent<GoToPose>();
            OSCWithMaster = OSCWithMasterObj.GetComponent<OscWithMaster>();
            OSCWithChataigne = OSCWithChataigneObj.GetComponent<OSCSendTime>();
            goToPose.setGoToInit();
            goToPose.goToTarget = false;
            OSCWithMaster.SendReadyStatus();
            animator.SetBool("Stop", false);
            OSCWithChataigne.oscGoTo(200);
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
