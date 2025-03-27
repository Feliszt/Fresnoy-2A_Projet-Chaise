using UnityEngine;
using UnityEngine.Events;

public class SerialReceiveMessage : MonoBehaviour
{

    private UnityEvent eventTest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (eventTest == null)
            eventTest = new UnityEvent();

        eventTest.AddListener(EventTestAction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EventTestAction()
    {
        Debug.Log("[EventTestAction] called");
    }
}
