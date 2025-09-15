using UnityEngine;

public class GoToFirst : StateMachineBehaviour
{
    public string MoverObjectName;
    public string FirstName;
    public string TargetName;
    private GoToPose goToPose;
    private RandomisePoses randomisePoses;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // get object dynamically
        GameObject mover = GameObject.Find(MoverObjectName);
        if (mover != null)
        {
            goToPose = mover.GetComponent<GoToPose>();
            randomisePoses = mover.GetComponent<RandomisePoses>();
            randomisePoses.randomisePoses = false;
            goToPose.setGoToFirst();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // get object dynamically
        GameObject first = GameObject.Find(FirstName);
        GameObject target = GameObject.Find(TargetName);
        target.transform.position = first.transform.position;
        target.transform.rotation = first.transform.rotation;
        //animator.SetBool("Play", false);
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
