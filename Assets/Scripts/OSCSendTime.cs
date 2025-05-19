using UnityEngine;
using extOSC;
using UnityEngine.Playables;
using UnityEditor.Timeline;

[ExecuteInEditMode]
public class OSCSendTime : MonoBehaviour
{
    public OSCTransmitter OSCTransmitter;
    public PlayableDirector timeline;
    private PlayState timelineStatePrev;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeline.state == PlayState.Playing && timelineStatePrev == PlayState.Paused)
        {
            var message = new OSCMessage("/goTo");
            message.AddValue(OSCValue.Float((float)timeline.time));
            OSCTransmitter.Send(message);

            message = new OSCMessage("/play");
            OSCTransmitter.Send(message);
        }

        if (timeline.state == PlayState.Paused && timelineStatePrev == PlayState.Playing)
        {
            var message = new OSCMessage("/pause");
            OSCTransmitter.Send(message);
        }



        // store state
        timelineStatePrev = timeline.state;
    }
}
