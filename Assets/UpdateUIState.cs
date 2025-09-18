using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUIState : MonoBehaviour
{
    public Animator stateMachine;
    private TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = transform.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText("State : {" + GetStateName() + "}");
    }

    string GetStateName()
    {
        if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("Start"))
        {
            return "Start";
        }
        if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("SetZero"))
        {
            return "SetZero";
        }
        if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("GoToInit"))
        {
            return "GoToInit";
        }
        if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("StandBy"))
        {
            return "StandBy";
        }
        if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            return "Idle";
        }
        if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("GoToFirst"))
        {
            return "GoToFirst";
        }
        if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("PlayTimeline"))
        {
            return "PlayTimeline";
        }
        return "UnknownState";
    }
}
