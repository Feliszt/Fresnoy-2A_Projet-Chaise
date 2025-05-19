using UnityEngine;
using extOSC;
using System;

public class OSCsendPlay : MonoBehaviour
{

    public OSCTransmitter OSCTransmitter;
    public Boolean b_sendPlay = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (b_sendPlay)
        {
            sendPlay();
            b_sendPlay = false;
        }
    }

    void sendPlay()
    {
        var message = new OSCMessage("/play");
        message.AddValue(OSCValue.Int(0));
        OSCTransmitter.Send(message);
    }
}
