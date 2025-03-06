using UnityEngine;
using System.Globalization;
using System;

[Serializable]
public class MotorSettings
{
    public bool enabled;
    public float speed;
    public float actualPosition;
    public float targetPosition;

}

public class BinomeMoteur : MonoBehaviour
{
    [Header("Informations")]
    public MotorSettings motor1Settings = new MotorSettings();
    public MotorSettings motor2Settings = new MotorSettings();
    public string codeVersion;

    [Header("Controls")]
    public bool getVersion;
    public bool resetPosition;
    public bool getPosition;
    public bool setEnabled;
    public bool setDisabled;

    public bool ping;

    [Header("Position control")]
    public int motor1TargetPosition;
    public int motor2TargetPosition;

    private int _previousMotor1TargetPosition;
    private int _previousMotor2TargetPosition;

    public int defaultSpeed = 100;
    public int defaultAcceleration = 100;

    [Header("Speed control")]
    public int motor1TargetSpeed;
    public int motor2TargetSpeed;
    private int _previousMotor1TargetSpeed;
    private int _previousMotor2TargetSpeed;

    [Header("Links")]
    public SerialPortManager serial;

    // Update is called once per frame
    void Update()
    {
        if(getVersion) {
            getVersion = false;
            GetVersion();
        }

        if(resetPosition) {
            resetPosition = false;
            ResetPosition();
        }
        
        if(getPosition) {
            getPosition = false;
            GetPosition();
        }

        if(setEnabled) {
            setEnabled = false;
            SetEnabled(true);
        }

        if(setDisabled) {
            setDisabled = false;
            SetEnabled(false);
        }

        if(ping) {
            ping = false;
            Ping();
        }

        if(motor1TargetPosition != _previousMotor1TargetPosition) {
            SetPosition(1, motor1TargetPosition);
            _previousMotor1TargetPosition = motor1TargetPosition;
        }

        if(motor2TargetPosition != _previousMotor2TargetPosition) {
            SetPosition(2, motor2TargetPosition);
            _previousMotor2TargetPosition = motor2TargetPosition;
        }

        if(motor1TargetSpeed != _previousMotor1TargetSpeed) {
            SetSpeed(1, motor1TargetSpeed);
            _previousMotor1TargetSpeed = motor1TargetSpeed;
        }

        if(motor2TargetSpeed != _previousMotor2TargetSpeed) {
            SetSpeed(2, motor2TargetSpeed);
            _previousMotor2TargetSpeed = motor2TargetSpeed;
        }
    }

    public void OnSerialMessage(string msg) {
        Debug.Log("[Binome] Got : " + msg);
        string[] strings= msg.Split(" ");
        if(strings.Length ==1) {return;}
        switch (strings[1]) {
            case "version":
                Debug.Log("[Binome] Version : " + strings[3]);
                codeVersion = strings[3];
            break;
            case "pos":
                if(strings[2] == "M1") {
                    motor1Settings.actualPosition = int.Parse(strings[4], CultureInfo.InvariantCulture);
                }

                if(strings[2] == "M2") {
                    motor2Settings.actualPosition = int.Parse(strings[4], CultureInfo.InvariantCulture);
                }
            break;
            case "enable":
                if(strings[2] == "done") {
                    motor1Settings.enabled = true;
                    motor2Settings.enabled = true;
                }
                break;
            case "disable":
                if(strings[2] == "done") {
                    motor1Settings.enabled = false;
                    motor2Settings.enabled = false; 
                }
                break;

        }
    }

    public void ResetPosition() {
        Debug.Log("[Binome] Reset position");
        serial.SendMessage("reset");
    }
    public void GetPosition() {
        Debug.Log("[Binome] Get position");
        serial.SendMessage("pos");
    }
    public void Ping() {
        Debug.Log("[Binome] Ping");
        serial.SendMessage("ping");
    }
    public void GetVersion() {
        Debug.Log("[Binome] Get version");
        serial.SendMessage("version");
    }

    public void SetPosition(int motorID, int targetPosition) {
        //Debug.Log("[Binome] Set position");
        serial.SendMessage("motorTo " + motorID  + " " + targetPosition + " " + defaultSpeed + " " + defaultAcceleration);
    }

    public void SetSpeed(int motorID, int targetSpeed) {
        //Debug.Log("[Binome] Set speed");
        serial.SendMessage("motorAt " + motorID  + " " + targetSpeed);
        
    }

    public void SetEnabled(bool enabled) {
        Debug.Log("[Binome] Set enabled : " + enabled);
        if(!enabled) 
            serial.SendMessage("disable");
        else
            serial.SendMessage("enable");
    }

    public void SetSpeedAndAcceleration(string speed) {
        defaultAcceleration = int.Parse(speed, CultureInfo.InvariantCulture);
        defaultSpeed = int.Parse(speed, CultureInfo.InvariantCulture);
    }
}
