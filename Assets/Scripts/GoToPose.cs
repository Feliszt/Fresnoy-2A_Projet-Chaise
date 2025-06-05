using System;
using Unity.VisualScripting;
using UnityEngine;

public class GoToPose : MonoBehaviour
{
    public Transform objectToMove;
    public Transform targetPose;
    public Transform zeroPose;
    public Transform firstPose;
    private Transform trueTargetPose;
    private Transform fromPose;
    [Range(0.0001f, 0.1f)]
    public float speed = 0.0006f;
    private float timeCount = 0.0f;
    public Boolean goToTarget = false;
    public Boolean goToZero = false;
    public Boolean goToFirst = false;
    private Boolean go = false;
    private Boolean goPrev = false;
    private Boolean going = false;
    private Boolean goingPrev = false;
    private float distToTarget;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trueTargetPose = zeroPose;
    }

    // Update is called once per frame
    void Update()
    {
        go = goToTarget ^ goToZero ^ goToFirst;

        if (!going && goToTarget) {
            trueTargetPose = targetPose;
            Debug.Log("go to target!");
        }

        if (!going && goToZero) {
            trueTargetPose = zeroPose;
            Debug.Log("go to zero!");
        }
        
        if (!going && goToFirst) {
            trueTargetPose = firstPose;
            Debug.Log("go to first!");
        }

        if (go && !goPrev)
        {
            timeCount = 0.0f;
            fromPose = objectToMove.transform;
            goToTarget = false;
            goToZero = false;
            goToFirst = false;
            go = false;
            going = true;
        }

        if (going) {
            objectToMove.position = Vector3.Lerp(fromPose.position, trueTargetPose.position, timeCount * speed);
            objectToMove.rotation = Quaternion.Lerp(fromPose.rotation, trueTargetPose.rotation, timeCount * speed);
        }

        distToTarget = Vector3.Distance(objectToMove.position, trueTargetPose.position) + Vector3.Distance(objectToMove.eulerAngles, trueTargetPose.eulerAngles);
        if (distToTarget <= 0.001f) {
            going = false;
            objectToMove.position = trueTargetPose.position;
            objectToMove.rotation = trueTargetPose.rotation;
        }

        if (!going && goingPrev) {
            Debug.Log("arrived at target.");
        }

        timeCount += Time.deltaTime;
        goPrev = go;
        goingPrev = going;
    }

    public bool isGoing() {
        return going;
    }

    public void setGoToZero() {
        goToZero = true;
    }

    public void setGoToTarget() {
        goToTarget = true;
    }
    
    public void setGoToFirst() {
        goToFirst = true;
    }

    public void setSpeed(float _speed)
    {
        speed = _speed;
    }
}
