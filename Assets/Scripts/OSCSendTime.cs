using UnityEngine;
using extOSC;
using UnityEngine.Playables;
//using UnityEditor.Timeline;

[ExecuteInEditMode]
public class OSCSendTime : MonoBehaviour
{
    public OSCTransmitter OSCTransmitter;
    public PlayableDirector timeline;
    public bool sendTime = false;
    private PlayState timelineStatePrev;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (sendTime)
        {
            oscGoTo();
            sendTime = false;
        }
        
        if (timeline.state == PlayState.Playing && timelineStatePrev == PlayState.Paused)
        {
            oscGoTo();
            oscPlay();
        }

        if (timeline.state == PlayState.Paused && timelineStatePrev == PlayState.Playing)
        {
            oscPause();
        }

        // store state
        timelineStatePrev = timeline.state;
    }

    void oscGoTo()
    {
        var message = new OSCMessage("/goTo");
        message.AddValue(OSCValue.Float((float)timeline.time));
        OSCTransmitter.Send(message);
    }

    void oscPlay()
    {
            var message = new OSCMessage("/play");
            OSCTransmitter.Send(message);
    }
    
    void oscPause()
    {
            var message = new OSCMessage("/pause");
            OSCTransmitter.Send(message);
    }
}
