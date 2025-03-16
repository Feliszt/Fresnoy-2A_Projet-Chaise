using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    public static FPSDisplay instance;
    public bool showFPS = false;
    public Color textColor = Color.white;
    public int offset = 10;
    public int fontSize = 15;
    public static float fps;

    public float fps_info;
    private GUIStyle _style;

    public float alertThreshold = -1.0f;

    public delegate void OnThresholdCrossed(float _fps);
    public static event OnThresholdCrossed onThresholdCrossed;

    private void Start()
    {
        instance = this;
        _style = new GUIStyle();
       // StartCoroutine(Outils.PeriodicallyCallAction(1.0f, CheckThreshold));
    }

    public void CheckThreshold()
    {
        if (alertThreshold != -1.0f)
        {
            if (fps < alertThreshold)
            {
                onThresholdCrossed?.Invoke(fps);
            }
        }
    }
    public void ToggleFPSDisplay()
    {
        showFPS = !showFPS;
    }

    private bool _keyToggled;

    void LateUpdate()
    {
        fps = (1.0f / Time.smoothDeltaTime);
        fps_info = fps;
    }

    void OnGUI()
    {
        if (showFPS) {
            _style.fontSize = fontSize;
            _style.normal.textColor = textColor;
            
            GUI.Label(new Rect(offset, offset, 100, 100), fps.ToString("F1"), _style);
        }
    }
}