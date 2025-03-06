using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SerialPortManager : MonoBehaviour
{
    [Header("Informations")]
    public bool isRunning = false;
    public bool pendingData;
    private float _previousDataTime;
    public float dataRate;

    [Header("Events")]
    public UnityEvent<string> OnMessageReceived;

    private SerialPort serialPort;
    private Thread serialThread;

    private ConcurrentQueue<string> _messages = new ConcurrentQueue<string>();

    [Header("Settings")]
    public string portName = "COM7";
    public int baudRate = 115200;
    public float autoConnectDelay = 2f;
    private float _previousConnectionAttemptTime = 0f;

    [Header("Controls")]
    public bool reconnect;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        UpdateMessages();

        if (reconnect) {
            reconnect = false;
            ClosePort();
            OpenPort();
        }

        if (!isRunning && Time.time - _previousConnectionAttemptTime > autoConnectDelay)
        {
            OpenPort();
            _previousConnectionAttemptTime = Time.time;
        }
      
    }

    public void UpdateMessages()
    {
        while (_messages.Count > 0)
        {
            pendingData = false;
            dataRate = 1f/ (Time.time-_previousDataTime);
           
            var message = "";
            _messages.TryDequeue(out message);

            // �met un �v�nement � chaque message re�u
            OnMessageReceived?.Invoke(message);
             _previousDataTime = Time.time;
        }
    }

    /// <summary>
    /// Initialise le gestionnaire du port s�rie.
    /// </summary>
    /// <param name="portName">Nom du port s�rie (ex: "COM3", "/dev/ttyUSB0").</param>
    /// <param name="baudRate">Vitesse du port s�rie en bauds (ex: 9600, 115200).</param>
    public void Initialize()
    {
        OpenPort();
    }

    /// <summary>
    /// Ouvre le port s�rie avec les param�tres d�finis.
    /// </summary>
    private void OpenPort()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.ReadTimeout = 1000;
            //serialPort.NewLine = ""+'\n';
            serialPort.Open();

            isRunning = true;
            serialThread = new Thread(ReadSerialPort);
            serialThread.Start();

            Debug.Log("Serial port opened : " + portName + " at " + baudRate + " bauds.");
        }
        catch (Exception e)
        {
            Debug.LogError("Erreur lors de l'ouverture du port série : " + e.Message);
        }
    }

    /// <summary>
    /// Ferme le port s�rie et arr�te le thread de lecture.
    /// </summary>
    public void ClosePort()
    {
        Debug.Log("[Serial] Closing port " + portName);
        try
        {
            isRunning = false;

            if (serialThread != null && serialThread.IsAlive)
            {
                serialThread.Join();
            }

            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                Debug.Log("Port série fermé.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Erreur lors de la fermeture du port serie : " + e.Message);
        }
    }

    /// <summary>
    /// Fonction appel�e en arri�re-plan pour lire les donn�es du port s�rie.
    /// </summary>
    private void ReadSerialPort()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string message = serialPort.ReadLine();
                if (!string.IsNullOrEmpty(message))
                {
                    pendingData = true;
                    
                    
                    //Debug.Log("Received : " + message);
                    _messages.Enqueue(message);
                  
                }
            }
            catch (TimeoutException)
            {
                // Timeout normal, pas d'action n�cessaire.
            }
            catch (Exception e)
            {
                Debug.LogError("Erreur lors de la lecture du port s�rie : " + e.Message);
                ClosePort();
            }
        }
    }

    /// <summary>
    /// Envoie un message via le port s�rie.
    /// </summary>
    /// <param name="message">Le message � envoyer.</param>
    public void SendMessage(string message)
    {
        Debug.Log("Sending : " +  message);
        try
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.WriteLine(message);
            }
            else
            {
                Debug.LogWarning("Le port s�rie est ferm� ou non initialis�.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Erreur lors de l'envoi du message via le port s�rie : " + e.Message);
        }
    }

    // Unity's MonoBehaviour lifecycle methods
    private void OnDestroy()
    {
        ClosePort();
    }

    private void OnApplicationQuit()
    {
        ClosePort();
    }
}