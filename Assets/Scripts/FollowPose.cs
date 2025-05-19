using UnityEngine;

[ExecuteInEditMode]
public class FollowPose : MonoBehaviour
{

    public Transform objectToMove;
    public Transform targetPose;

    [Range(0.01f, 10.0f)]
    public float speed = 0.01f;
    private Transform fromPose;
    private float timeCount = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fromPose = objectToMove.transform;
    }

    // Update is called once per frame
    void Update()
    {
        objectToMove.position = Vector3.Lerp(fromPose.position, targetPose.position, Time.deltaTime * speed);
        objectToMove.rotation = Quaternion.Lerp(fromPose.rotation, targetPose.rotation, Time.deltaTime * speed);
    }
}
