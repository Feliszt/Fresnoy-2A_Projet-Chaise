using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(filinsHandler))]
public class filinsHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        filinsHandler _filinsHandler = (filinsHandler)target;

        if (GUILayout.Button("Zero"))
        {
            _filinsHandler.Zero();
        }
    }
}
