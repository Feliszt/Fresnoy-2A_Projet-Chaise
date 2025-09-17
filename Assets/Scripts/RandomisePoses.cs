using System;
using System.Drawing;
using UnityEngine;

public class RandomisePoses : MonoBehaviour
{
    public Vector3 areaPosition = new Vector3(0.0f, 0.5f, 0.0f);
    public Vector3 areaSize = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector2 rotXLimits = new Vector2(-10.0f, 10.0f);
    public Vector2 rotYLimits = new Vector2(-10.0f, 10.0f);
    public Vector2 rotZLimits = new Vector2(-10.0f, 10.0f);
    public Boolean randomisePoses = false;
    private GoToPose goToPose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goToPose = transform.GetComponent<GoToPose>();
    }

    // Update is called once per frame
    void Update()
    {
        if (randomisePoses && !goToPose.isGoing())
        {
            goToPose.targetPose.position = getRandomPosition().Item1;
            goToPose.targetPose.eulerAngles = getRandomPosition().Item2;
            goToPose.goToTarget = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new UnityEngine.Color(1.0f, 0.0f, 0.0f, 1.0f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(areaPosition, areaSize);
    }

    Tuple<Vector3, Vector3> getRandomPosition()
    {
        Vector3 randomPosition = areaPosition + new Vector3(
            UnityEngine.Random.Range(-areaSize.x / 2, areaSize.x / 2),
            UnityEngine.Random.Range(-areaSize.y / 2, areaSize.y / 2),
            UnityEngine.Random.Range(-areaSize.z / 2, areaSize.z / 2)
                                                            );
        Vector3 randomRotation = new Vector3(
            UnityEngine.Random.Range(rotXLimits.x, rotXLimits.y),
            UnityEngine.Random.Range(rotYLimits.x, rotYLimits.y),
            UnityEngine.Random.Range(rotZLimits.x, rotZLimits.y)
        );
        return new Tuple<Vector3, Vector3>(randomPosition, randomRotation);
    }

    public void toggleRandomizePoses()
    {
        randomisePoses = !randomisePoses;
    }
    
    public void setRandomizePosesFalse()
    {
        randomisePoses = false;
    }
}
