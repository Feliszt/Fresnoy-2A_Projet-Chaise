using System;
using UnityEngine;

[Serializable]
public class BinomeMoteurFilin {
    public string name;
    public lineController[] lines;

    [Range(-1000, 1000)]
    public int motor1Offset;
    [Range(-1000, 1000)]
    public int motor2Offset;
    public float[] distances;
    public float[] derivatedSpeed;
    public float[] previousSpeedValue;
    public float speedOutThreshold = 30f;
    public float[] previousDistances;
    public int[] motorSteps;
    public int[] motorStepsZero;
    public BinomeMoteur binomeMoteur;
}

public class filinsHandler : MonoBehaviour
{

    [Header("Motor control")]
    public bool setZero;
    public bool sendPos = false;
    public bool sendSpeed = false;
    public float targetFps;
    private float previousTime;
    private float previousSpeedComputationTime;

    [Header("Links")]
    public BinomeMoteurFilin[] binomeMoteurFilins;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 240;
        foreach (var b in binomeMoteurFilins)
        {
            b.distances = new float[b.lines.Length];
            b.derivatedSpeed = new float[b.lines.Length];
            b.previousDistances = new float[b.lines.Length];
            b.motorSteps = new int[b.lines.Length];
            b.motorStepsZero = new int[b.lines.Length];
            b.previousSpeedValue = new float[b.lines.Length];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (setZero)
        {
            setZero = false;
            Zero();
        }

        
        if (Time.time - previousTime >=  (1.0f / (float)targetFps))
        {
            UpdateMotorSteps();
            UpdateMotorSpeeds();

            if (sendPos)
            {
                if(sendSpeed) {
                    sendSpeed =false;
                }

                sendPosToMotors();
            }


            if (sendSpeed)
            {
                if(sendPos) {
                    sendPos =false;
                }
                SendSpeedToMotors();
            }
            previousTime = Time.time;
        }
    }

    //
    public void Zero()
    {
        foreach (var b in binomeMoteurFilins)
        {
            for (int i = 0; i < b.lines.Length; i++)
            {
                b.distances[i] = b.lines[i].distanceBetween;
                b.previousDistances[i] = b.distances[i];

                var ropeLengthPerRev = 0.09425f; // m
                var nbStepsPerRev = 800.0f;
                b.motorStepsZero[i] = (int)(b.distances[i] * nbStepsPerRev / ropeLengthPerRev);
                b.derivatedSpeed[i] = 0;
                b.previousSpeedValue[i] = 0;
            }
        }
    }

    //
    public void UpdateMotorSteps()
    {
        foreach (var b in binomeMoteurFilins)
        {
            for (int i = 0; i < b.lines.Length; i++) {
                b.distances[i] = b.lines[i].distanceBetween;
                var ropeLengthPerRev = 0.09425f; //m
                var nbStepsPerRev = 800.0f;

                b.motorSteps[i] = (int)(b.distances[i] * nbStepsPerRev / ropeLengthPerRev) - b.motorStepsZero[i];
                b.motorSteps[i] = -b.motorSteps[i];

                if(i==0) {
                    b.motorSteps[i] += b.motor1Offset;
                }
                if(i==1) {
                    b.motorSteps[i] += b.motor2Offset;
                }

                if (Mathf.Approximately(b.motorSteps[i], 0f))
                {
                    b.motorSteps[i] = 0;
                }
            }
        }
        stepsData += (binomeMoteurFilins[0].motorSteps[0] + "\n");
    }

    public void UpdateMotorSpeeds()
    {
        var deltaTime = Time.time - previousSpeedComputationTime;
        foreach (var b in binomeMoteurFilins)
        {
            for (int i = 0; i < b.lines.Length; i++)
            {
                
                b.distances[i] = b.lines[i].distanceBetween;
                float speed = (b.distances[i] - b.previousDistances[i]) / deltaTime; // m/s
                
                float ropeLengthPerRev = 0.09425f; // m
                float nbStepsPerRev = 800.0f;
                b.derivatedSpeed[i] = -1.0f * (int)(speed * nbStepsPerRev / ropeLengthPerRev); // Steps per second

                if (Mathf.Approximately(b.derivatedSpeed[i], 0f))
                {
                    b.derivatedSpeed[i] = 0;
                }

                if(Mathf.Abs(b.derivatedSpeed[i] - b.previousSpeedValue[i]) >= b.speedOutThreshold) {
                    Debug.LogError("SKIPPED VALUE ! Was : " + b.derivatedSpeed[i] + "  setting to " + b.previousSpeedValue[i]);
                    b.derivatedSpeed[i] = b.previousSpeedValue[i];
                }
                else {
                    b.previousSpeedValue[i] = b.derivatedSpeed[i];
                }

                b.previousDistances[i] = b.distances[i];
            }
        }
        previousSpeedComputationTime = Time.time;
        speedData += (binomeMoteurFilins[0].derivatedSpeed[0] + "\n");
    }

    public void SendSpeedToMotors()
    {
        foreach (var b in binomeMoteurFilins)
        {
            b.binomeMoteur.motor1TargetSpeed = (int)b.derivatedSpeed[0];
            b.binomeMoteur.motor2TargetSpeed = (int)b.derivatedSpeed[1];
            
            // for (int i = 0; i < b.lines.Length; i++)
            // {
            //     b.binomeMoteur.SetSpeed(i+1, (int)b.derivatedSpeed[i]);
            // }
        }
    }
    private string speedData;
    private string stepsData;
    public void OnApplicationQuit()
    {
        Debug.LogWarning("speedData" + speedData);
        Debug.LogWarning("stepsData" + stepsData);
    }
    public void sendPosToMotors()
    {
        foreach (var b in binomeMoteurFilins)
        {
            b.binomeMoteur.motor1TargetPosition = b.motorSteps[0];
            b.binomeMoteur.motor2TargetPosition = b.motorSteps[1];
        }
    }

    public void SetFramerate(string framerate) {
        targetFps = int.Parse(framerate, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void SetSendPos(bool state) {
        sendPos = state;
    }

    public void SetSendSpeed(bool state) {
        sendSpeed = state;
    }

    public void SetZero() {
        Zero();
    }
}
