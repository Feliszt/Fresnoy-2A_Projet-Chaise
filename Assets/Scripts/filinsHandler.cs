using System;
using UnityEngine;

[Serializable]
public class BinomeMoteurFilin {
    public string name;
    public lineController[] lines;

    [Range(-5000, 5000)]
    public int motor1Offset;
    [Range(-5000, 5000)]
    public int motor2Offset;
    public bool motor1Inverse;
    public bool motor2Inverse;
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
    [Header("Motor settings")]
    public int stepsPerRev = 800;
    public float ropeLengthPerRevHigh = 0.1075f;
    public float ropeLengthPerRevLow = 0.1075f;

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

        /*foreach(var b in binomeMoteurFilins)
        {
            b.motor1Offset = forcedMotorOffset;
            b.motor2Offset = forcedMotorOffset;
        }*/
        
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
                if (i == 0) {
                    b.motorStepsZero[i] = 1 * (int)(b.distances[i] * stepsPerRev / ropeLengthPerRevHigh);
                }
                if (i == 1) {
                    b.motorStepsZero[i] = 1 * (int)(b.distances[i] * stepsPerRev / ropeLengthPerRevLow);
                }
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

                if (i == 0) {
                    b.motorSteps[i] =  1 * (int)(b.distances[i] * stepsPerRev / ropeLengthPerRevHigh) - b.motorStepsZero[i];
                    b.motorSteps[i] += 1 * b.motor1Offset;
                }
                if (i == 1) {
                    b.motorSteps[i] =  1 * (int)(b.distances[i] * stepsPerRev / ropeLengthPerRevLow) - b.motorStepsZero[i];
                    b.motorSteps[i] += 1 * b.motor2Offset;
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
                float computedSpeed = 1 * (int)(speed * stepsPerRev / ropeLengthPerRevHigh); // Steps per second
                
                // Optionnel : Si le delta absolu dépasse trop le seuil, on peut ne pas l'appliquer complètement
                if(Mathf.Abs(computedSpeed - b.previousSpeedValue[i]) >= b.speedOutThreshold)
                {
                    //Debug.LogError($"SKIPPED VALUE for cable {i}! Computed: {computedSpeed} ; Using value: {b.previousSpeedValue[i]}");
                    // On peut choisir de conserver la vitesse précédente ou d'appliquer un lissage partiel
                    computedSpeed = b.previousSpeedValue[i];
                }

                b.derivatedSpeed[i] = computedSpeed;
                // Mise à jour de la valeur précédente pour le prochain cycle
                b.previousSpeedValue[i] = computedSpeed;
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
            if (b.motor1Inverse) {
                b.binomeMoteur.motor1TargetSpeed = - (int)b.derivatedSpeed[0];
            } else {
                b.binomeMoteur.motor1TargetSpeed = (int)b.derivatedSpeed[0];
            }

            if (b.motor2Inverse) {
                b.binomeMoteur.motor2TargetSpeed = - (int)b.derivatedSpeed[1];
            } else {
                b.binomeMoteur.motor2TargetSpeed = (int)b.derivatedSpeed[1];
            }
            
            //b.binomeMoteur.motor1TargetSpeed = (int)b.derivatedSpeed[0];
            //b.binomeMoteur.motor2TargetSpeed = (int)b.derivatedSpeed[1];
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
            if (b.motor1Inverse) {
                b.binomeMoteur.motor1TargetPosition = - b.motorSteps[0];
            } else {
                b.binomeMoteur.motor1TargetPosition = b.motorSteps[0];
            }
            
            if (b.motor2Inverse) {
                b.binomeMoteur.motor2TargetPosition = - b.motorSteps[1];
            } else {
                b.binomeMoteur.motor2TargetPosition = b.motorSteps[1];
            }
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
