using UnityEngine;

public class filinsHandler : MonoBehaviour
{
    public lineController[] lines;
    public float[] distances;
    public int[] motorSteps;


    [Header("Motor control")]
    private int[] motorStepsZero;
    public bool setZero;
    public bool sendPos = false;
    public int targetFps;
    private float previousTime;

    [Header("Links")]
    public BinomeMoteur[] ESP;
    public string[] filinToMotor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distances = new float[lines.Length];
        motorSteps = new int[lines.Length];
        motorStepsZero = new int[lines.Length];
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
        for (int i = 0; i < lines.Length; i++)
        {
            motorStepsZero[i] = (int)(distances[i] * 4244f);
        }
    }

    //
    public void setMotorSteps()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            distances[i] = lines[i].distanceBetween;
            motorSteps[i] = (int)(distances[i] * 4244f) - motorStepsZero[i];
            motorSteps[i] = -motorSteps[i];

            if (Mathf.Approximately(motorSteps[i], 0f))
            {
                motorSteps[i] = 0;
            }
        }
    }

    public void sendPosToMotors()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            // get ESP and motor
            int ESP_index = int.Parse(filinToMotor[i].Split("-")[0]);
            int motor_index = int.Parse(filinToMotor[i].Split("-")[1]);

            // send target pos
            ESP[ESP_index - 1].SetPosition(motor_index, motorSteps[i]);
        }
    }
}
