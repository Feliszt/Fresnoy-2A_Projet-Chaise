using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformObject : MonoBehaviour
{
    public CalibrateSpace calibration;
    public Transform inputTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (calibration.getTransformMatrix() != null) {
            transform.position = Matrix4x4.Inverse(calibration.getTransformMatrix()).MultiplyPoint3x4(inputTransform.position);
            transform.rotation = Matrix4x4.Inverse(calibration.getTransformMatrix()).rotation * inputTransform.rotation;
        }
    }
}
