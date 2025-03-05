using UnityEngine;

public class lookAt : MonoBehaviour
{

    public Transform objectToLook;
    public float offset;
    public bool reverse;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist1 = objectToLook.position.z - transform.position.z;
        float dist2 = Vector2.Distance(new Vector2(objectToLook.position.x, objectToLook.position.z), new Vector2(transform.position.x, transform.position.z));
        float angle = Mathf.Asin(dist1 / dist2);

        Vector3 newRotation = transform.rotation.eulerAngles;
        if (reverse) angle = -angle;
        newRotation.y = angle * Mathf.Rad2Deg + offset;
        transform.rotation = Quaternion.Euler(newRotation);
    }
}
