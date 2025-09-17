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
        OSCReceiver.Bind("/Connect", OSCMasterConnect);
        OSCReceiver.Bind("/Felix/Ready", OSCMasterReady);
        OSCReceiver.Bind("/Felix/Play", OSCMasterPlay);
        OSCReceiver.Bind("/Felix/Stop", OSCMasterStop);
    }

    private void OSCMasterConnect(OSCMessage messageIn)
    {
        Debug.Log("[OSCWITHMASTER] -> message : Connect");
        var messageOut = new OSCMessage("/Felix/Connect");
        messageOut.AddValue(OSCValue.Bool(true));
        OSCTransmitter.Send(messageOut);
    }

    private void OSCMasterReady(OSCMessage messageIn)
    {
        Debug.Log("[OSCWITHMASTER] -> message : Ready");
        SendReadyStatus();
    }

    private void OSCMasterPlay(OSCMessage messageIn)
    {
        Debug.Log("[OSCWITHMASTER] -> message : Play");
        stateMachine.SetBool("Play", true);
    }

    
    private void OSCMasterStop(OSCMessage messageIn)
    {
        Debug.Log("[OSCWITHMASTER] -> message : Stop");
        stateMachine.SetBool("Stop", true);
    }

    public void SendReadyStatus()
    {
        var messageOut = new OSCMessage("/Felix/Ready");
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
