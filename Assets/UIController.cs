using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button myButton; // Assign in Inspector
    public ObjectToControl targetScript; // The script that contains the bool

    private void Start()
    {
        // Attach button click event
        myButton.onClick.AddListener(ToggleBoolean);
    }

    private void ToggleBoolean()
    {
        if (targetScript != null) {
            targetScript.someBool = !targetScript.someBool;
            Debug.Log("Bool Toggled to : " + targetScript.someBool);
        }
    }
}
