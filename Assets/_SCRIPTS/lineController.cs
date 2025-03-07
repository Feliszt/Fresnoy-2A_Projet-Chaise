using UnityEngine;

[ExecuteInEditMode]
public class lineController : MonoBehaviour
{
    public Transform fromPoint;
    public Transform toPoint;
    public float distanceBetween;

    private LineRenderer line;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line = GetComponent<LineRenderer>();

        SetPositions();
    }

    // Update is called once per frame
    void Update()
    {
        SetPositions();
    }

    void SetPositions()
    {
        line.SetPosition(0, fromPoint.position);
        line.SetPosition(1, toPoint.position);
        distanceBetween = Vector3.Distance(fromPoint.transform.position, toPoint.transform.position);
    }
}
