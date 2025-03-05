using UnityEngine;

public class filinsHandler : MonoBehaviour
{
    public lineController[] lines;
    public float[] distances;
    public float[] motorSteps;

    private float[] motorStepsZero;
    private bool first = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distances = new float[lines.Length];
        motorSteps = new float[lines.Length];
        motorStepsZero = new float[lines.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (first)
        {
            first = false;
            for (int i = 0; i < lines.Length; i++)
            {
                
            }
        }

        for (int i = 0; i < lines.Length; i++)
        {
            distances[i] = lines[i].distanceBetween;
            motorSteps[i] = motorStepsZero[i] - distances[i] * 4244f;

            if (Mathf.Approximately(motorSteps[i], 0f))
            {
                motorSteps[i] = 0f;
            }
        }
    }

    //
    public void Zero()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            motorStepsZero[i] = distances[i] * 4244f;
        }
    }
}
