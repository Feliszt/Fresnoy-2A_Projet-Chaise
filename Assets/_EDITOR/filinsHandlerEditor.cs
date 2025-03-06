using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(filinsHandler))]
public class filinsHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
