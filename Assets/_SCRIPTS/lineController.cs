using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class lineController : MonoBehaviour
{
    public GameObject distanceTextPrefab;
    public Transform fromPoint;
    public Transform toPoint;
    public float distanceBetween;

    private LineRenderer line;
    private TMP_Text _distanceText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        line = GetComponent<LineRenderer>();

        for(int i = 0; i < transform.childCount; i++) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        _distanceText = Instantiate(distanceTextPrefab).GetComponent<TMP_Text>();
        _distanceText.transform.parent = transform;
        //set _distanceText position at line center + vertical offset and oriented like the line
        _distanceText.transform.localPosition = (fromPoint.position + toPoint.position) / 2f + Vector3.up * 0.075f;
        _distanceText.text = (distanceBetween * 100.0f).ToString("F1") + " cm";

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

        _distanceText.text = (distanceBetween * 100.0f).ToString("F1") + " cm";
        _distanceText.transform.localRotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
        _distanceText.transform.localPosition = (fromPoint.position + toPoint.position) / 2f + Vector3.up * 0.075f;
    }
}
