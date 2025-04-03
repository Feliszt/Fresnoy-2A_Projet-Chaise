using UnityEngine;


public class LoadESPFromJson : MonoBehaviour
{
    public string settingsFileName;
    private string settingsFilePath = Application.dataPath + "/Data/";
    private SerialPortManager ESPPortManager;

    private class ESPMapping 
    {
        public string ESP32_SudEst;
        public string ESP32_SudOuest;
        public string ESP32_NordEst;
        public string ESP32_NordOuest;

        public string getByName(string ESPName) {
            if(ESPName == "ESP32_SudEst")
                return ESP32_SudEst;
            if(ESPName == "ESP32_SudOuest")
                return ESP32_SudOuest;
            if(ESPName == "ESP32_NordEst")
                return ESP32_NordEst;
            if(ESPName == "ESP32_NordOuest")
                return ESP32_NordOuest;
            return null;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {  
        // load file
        settingsFilePath += settingsFileName + ".json";
        if (!System.IO.File.Exists(settingsFilePath))
		{
            Debug.LogWarning("Could not find ESP settings file.");
            return;
        }
        ESPMapping mapping = JsonUtility.FromJson<ESPMapping>(System.IO.File.ReadAllText(settingsFilePath));

        // set all com ports in children
        foreach (Transform child in transform) {
            // check if child has component
            ESPPortManager = child.GetComponent<SerialPortManager>();
            if (!ESPPortManager) {
                Debug.LogWarning("Could not find SerialPortManager component in ESP game object.");
                return;
            }

            // set com port
            string comPort = mapping.getByName(child.name);
            if (comPort != null) {
                ESPPortManager.portName = comPort;
            }
        }
    }
}
