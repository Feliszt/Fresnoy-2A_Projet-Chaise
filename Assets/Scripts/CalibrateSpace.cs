using System;
using UnityEngine;
using Valve.VR;
using UnityEngine.XR;
using Unity.VisualScripting;


public class CalibrateSpace : MonoBehaviour
{
	// input trigger
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.LeftHand;
	public Transform inputController;
	public Transform inputControllerCalibrated;
	public Boolean calibrationMode = false;
	public Boolean calibrate;
	private Transform inputTransform, inputTransformCalibrated;
    private float trigger, triggerPrev;
	private int helperPointIndex = 1;

	// points
    public Transform[] sourcePoints = new Transform[0];
	public Transform[] targetPoints;
	public int calibrationPointIndex;

	// calibration and matrix
	private Matrix4x4 transformMatrix;
	private Boolean calibrated = false;
	private GameObject transformedTracker;
	private string calibrationFilePath = Application.dataPath + "/Data/calibration.json";
		
    // Use this for initialization
    public void Start () 
	{
		// get pointer object on controller
		inputTransform = null;
		if (inputController) {
			inputTransform = inputController.Find("Pointer");
		}

		// get pointer object on calibrated controller
		inputTransformCalibrated = null;
		if (inputControllerCalibrated) {
			inputTransformCalibrated = inputControllerCalibrated.Find("PointerTransformed");
		}

		// init index and matrix
		calibrationPointIndex = 0;
		transformMatrix = new Matrix4x4();
		if (System.IO.File.Exists(calibrationFilePath))
		{
            transformMatrix = JsonUtility.FromJson<Matrix4x4>(System.IO.File.ReadAllText(calibrationFilePath));
			for (int i = 0; i < targetPoints.Length; i++)
			{
                targetPoints[i].position = transformMatrix.MultiplyPoint3x4(sourcePoints[i].position);
                targetPoints[i].rotation = transformMatrix.rotation * sourcePoints[i].rotation;
            }
        }
	}

	private void Update()
	{
		if (calibrate){
			calibrate = !calibrate;
			Calibrate();
		}

		if (!inputTransform) {
			Debug.LogWarning("input transform not able to be set.");
		}

        // detect trigger
        trigger = SteamVR_Actions._default.Squeeze.GetAxis(inputSource); 
        if (trigger >= 1.0f && triggerPrev < 1.0f)
        {
			//Debug.Log(inputTransform.position);
			if (calibrationMode) {
				AddTargetPoint(inputTransform);
			} else {
				AddHelperPoint(inputTransformCalibrated);
			}
        }
        triggerPrev = trigger;

		//
		if (calibrated)
		{
            transformedTracker.transform.position = Matrix4x4.Inverse(transformMatrix).MultiplyPoint3x4(inputTransform.position);
            transformedTracker.transform.rotation = Matrix4x4.Inverse(transformMatrix).rotation * inputTransform.rotation;
        }
    }

	public void AddTargetPoint(Transform pointTransform)
	{

		if (targetPoints[calibrationPointIndex] != null)
		{
			targetPoints[calibrationPointIndex].SetPositionAndRotation(new Vector3(pointTransform.position.x, pointTransform.position.y, pointTransform.position.z), Quaternion.identity);
			calibrationPointIndex += 1;
			//Debug.Log("Adding " + targetPoints[calibrationPointIndex].transform.position + "\t" + calibrationPointIndex);
		}

		if (calibrationPointIndex >= sourcePoints.Length)
		{
			calibrationPointIndex = 0;
		}
	}

	public void AddHelperPoint(Transform pointTransform) {
		Transform helperParent = GameObject.Find("HelperPoints").transform;
		GameObject helperPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		helperPoint.name = "Helper point #" + helperPointIndex.ToString();
		helperPoint.transform.position = pointTransform.position;
		helperPoint.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		helperPoint.transform.parent = helperParent.transform;
		helperPointIndex++;
	}

	public void Calibrate()
	{
		//
        Debug.Log("Calibrating");
        transformMatrix = CalculateAlignmentTransform();
        string calibrationString = JsonUtility.ToJson(transformMatrix);
        System.IO.File.WriteAllText(Application.dataPath + "/Data/calibration.json", calibrationString);

        //
        transformedTracker = new GameObject();
        transformedTracker.name = "inputTrackerTransformed";
        calibrated = true;
    }

	public Matrix4x4 getTransformMatrix()
	{
		return transformMatrix;
	}

    private float CalculateCalibrationDistance()
    {
		float result = 0;

		for(int i = 0; i < sourcePoints.Length; i++)
        {
			result += Vector3.Distance(sourcePoints[i].position, targetPoints[i].position);
        }

		result = result / sourcePoints.Length;
		return result;
    }

	public Matrix4x4 CalculateAlignmentTransform()
	{
		Vector3[] sourcePointsPosition = new Vector3[sourcePoints.Length];
		Vector4[] targetPointsPosition = new Vector4[sourcePoints.Length];;

		for (int i = 0; i < sourcePoints.Length; i++)
		{
			sourcePointsPosition[i] = sourcePoints[i].position;
			targetPointsPosition[i] = new Vector4(targetPoints[i].position.x, targetPoints[i].position.y, targetPoints[i].position.z, 1);
		}

		return KabschSolver.SolveKabsch(sourcePointsPosition, targetPointsPosition);
	}
}