using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpdateToggleUI : MonoBehaviour
{
    private Toggle attachedToggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attachedToggle = GetComponent<Toggle>();
        if (attachedToggle == null)
        {
            Debug.Log("No Toggle component attached to this object.");
            return;
        }

        Toggle.ToggleEvent events = attachedToggle.onValueChanged;
        for (int i = 0; i < events.GetPersistentEventCount(); i++)
        {
            Object obj = events.GetPersistentTarget(i);
            string target = events.GetPersistentMethodName(i);
            UnityEventCallState callState = events.GetPersistentListenerState(i);

            

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
