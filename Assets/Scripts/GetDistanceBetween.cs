using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class GetDistanceBetween : MonoBehaviour
{
    public Transform object1;
    public Transform object2;
    public float distanceBetween;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceBetween = Vector3.Distance(object1.position, object2.position);
    }
}
