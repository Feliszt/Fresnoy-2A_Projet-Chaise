using UnityEngine;

[ExecuteInEditMode]
public class changePivotPoint : MonoBehaviour
{
    public Transform child;
    public Vector3 pivotPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pivotPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pivotPoint;
        child.localPosition = -pivotPoint;
    }
}
