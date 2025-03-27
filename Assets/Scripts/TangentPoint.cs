using UnityEngine;

[ExecuteInEditMode]
public class TangentPoint : MonoBehaviour
{
    public float poulieRadius = 0.1f;
    public Transform targetPoint;

    public Transform pouliePoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ComputePouliePoint();
    }

    //compute pouliePOoint position according to TargetPoint position and poulieRadius
    public void ComputePouliePoint() {
        // Centre de la poulie
        Vector3 center = transform.position;
        // Position du point cible
        Vector3 targetPos = targetPoint.position;
        // Vecteur allant du centre à la cible
        Vector3 d = targetPos - center;
        float dist = d.magnitude;

        // Si la cible est trop proche (inférieure au rayon), on positionne le point sur le cercle dans la direction de d
        if (dist <= poulieRadius)
        {
            pouliePoint.position = center + d.normalized * poulieRadius;
            return;
        }

        // Angle entre d et la tangente, tel que cos(theta) = poulieRadius/dist
        float theta = Mathf.Acos(poulieRadius / dist); // en radians

        // On choisit un axe de rotation, ici perpendiculaire à d et à l'axe Up (à adapter si besoin)
        Vector3 axis = Vector3.Cross(d, Vector3.up);
        if (axis == Vector3.zero) // cas particulier : d est colinéaire à Vector3.up
        {
            axis = Vector3.forward;
        }
        axis.Normalize();

        // On tourne d.normalized de theta (converti en degrés) autour de cet axe pour obtenir la direction de la tangente
        Vector3 tangentDir = Quaternion.AngleAxis(theta * Mathf.Rad2Deg, axis) * d.normalized;

        // Le point de tangence est alors sur le cercle : 
        Vector3 tangentPoint = center + tangentDir * poulieRadius;
        pouliePoint.position = tangentPoint;


        
    }
}
