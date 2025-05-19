using System.Linq;
using UnityEngine;

public class changePivotPoint : MonoBehaviour
{
    public Transform objects;
    public Vector3 pivotPoint;
    private Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = objects.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        objects.localPosition = startPos + pivotPoint;
        objects.GetChild(0).transform.localPosition = startPos - pivotPoint;
    }
}
