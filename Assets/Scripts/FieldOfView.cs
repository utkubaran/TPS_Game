using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Field of View Parameters")]
    [Space]
    public float viewRadius;
    
    [SerializeField, Range(0f, 360f)]
    public float viewAngle;

    [SerializeField]
    LayerMask targetMask, obstacleMask;

    [SerializeField]
    Transform viewVisualisationPoint;

    private bool isTargetInSight;
    public bool IsTargetInSight {get { return isTargetInSight; } }

    private void Update()
    {
        CheckTargetsInFOW();
        Debug.Log(isTargetInSight);
    }

    private void CheckTargetsInFOW()
    {
        if (Physics.CheckSphere(viewVisualisationPoint.position, viewRadius, targetMask))
        {
            FindVisibleTargets();
        }
        else
        {
            isTargetInSight = false;
        }
    }

    private void FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(viewVisualisationPoint.position, viewRadius, targetMask);

        foreach ( var targetInViewRadius in targetsInViewRadius)
        {
            Transform target = targetInViewRadius.transform;
            Vector3 directionToTarget = (target.position - viewVisualisationPoint.position).normalized;
            float distanceToTarget = Vector3.Distance(viewVisualisationPoint.position, target.position);

            if (Vector3.Angle(transform.forward, directionToTarget) <= viewAngle / 2f && distanceToTarget <= viewRadius)
            {

                isTargetInSight = !Physics.Raycast(viewVisualisationPoint.position, directionToTarget, distanceToTarget, obstacleMask);
            }
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool isAngleGlobal)
    {
        if (!isAngleGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
