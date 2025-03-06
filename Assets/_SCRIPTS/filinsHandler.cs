using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BinomeMoteurFilin {
    public lineController[] lines;
    public float[] distances;
    public int[] motorSteps;
    public int[] motorStepsZero;
    public BinomeMoteur binomeMoteur;
}

public class filinsHandler : MonoBehaviour
{

    [Header("Motor control")]
    public bool setZero;
    public bool sendPos = false;
    public int targetFps;
    private float previousTime;

    [Header("Links")]
    public BinomeMoteurFilin[] binomeMoteurFilins;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var b in binomeMoteurFilins)
        {
            b.distances = new float[b.lines.Length];
            b.motorSteps = new int[b.lines.Length];
            b.motorStepsZero = new int[b.lines.Length];
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

        setMotorSteps();

        if (sendPos)
        {
            if (Time.time - previousTime >=  1.0 / targetFps)
            {
                Debug.Log("send Pos : " + (Time.time - previousTime));
                sendPosToMotors();
                previousTime = Time.time;
            }

        }
    }

    //
    public void Zero()
    {
        foreach (var b in binomeMoteurFilins)
        {
            for (int i = 0; i < b.lines.Length; i++) {
                var ropeLengthPerRev = 0.09425f; //m
                var nbStepsPerRev = 800.0f;
                b.motorStepsZero[i] = (int)(b.distances[i] * nbStepsPerRev / ropeLengthPerRev);
            }
        }
    }

    //
    public void setMotorSteps()
    {
        foreach (var b in binomeMoteurFilins)
        {
            for (int i = 0; i < b.lines.Length; i++) {
                b.distances[i] = b.lines[i].distanceBetween;
                var ropeLengthPerRev = 0.09425f; //m
                var nbStepsPerRev = 800.0f;

                b.motorSteps[i] = (int)(b.distances[i] * nbStepsPerRev / ropeLengthPerRev) - b.motorStepsZero[i];
                b.motorSteps[i] = -b.motorSteps[i];

                if (Mathf.Approximately(b.motorSteps[i], 0f))
                {
                    b.motorSteps[i] = 0;
                }
            }
        }
    }

    public void sendPosToMotors()
    {
        foreach (var b in binomeMoteurFilins)
        {
            for (int i = 0; i < b.lines.Length; i++)
            {
                b.binomeMoteur.SetPosition(i+1, b.motorSteps[i]);
            }
        }
    }
}
