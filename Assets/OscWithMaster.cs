using UnityEngine;
using extOSC;

public class OscWithMaster : MonoBehaviour
{
    private OSCTransmitter OSCTransmitter;
    private OSCReceiver OSCReceiver;
    public Animator stateMachine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OSCTransmitter = transform.GetComponent<OSCTransmitter>();
        OSCReceiver = transform.GetComponent<OSCReceiver>();

        // bind messages
        OSCReceiver.Bind("/felix/ping", OSCMasterPing);
        OSCReceiver.Bind("/felix/ready", OSCMasterReady);
        OSCReceiver.Bind("/felix/go", OSCMasterGo);
    }

    private void OSCMasterPing(OSCMessage messageIn)
    {
        var messageOut = new OSCMessage("/felix/pong");
        messageOut.AddValue(OSCValue.Bool(true));
        OSCTransmitter.Send(messageOut);
    }

    private void OSCMasterReady(OSCMessage messageIn)
    {
        SendReadyStatus();
    }

    private void OSCMasterGo(OSCMessage messageIn)
    {
        stateMachine.SetBool("Play", true);
    }

    public void SendReadyStatus()
    {
        var messageOut = new OSCMessage("/felix/ready");
        if (stateMachine.GetCurrentAnimatorStateInfo(0).IsName("GoToFirst") || stateMachine.GetCurrentAnimatorStateInfo(0).IsName("PlayTimeline"))
        {
            messageOut.AddValue(OSCValue.Bool(false));
        }
        else
        {
            messageOut.AddValue(OSCValue.Bool(true));
        }
        OSCTransmitter.Send(messageOut);
    }
}
