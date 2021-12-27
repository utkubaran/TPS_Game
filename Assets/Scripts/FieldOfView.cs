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

    [Header("Field of View Visualisation")]
    [SerializeField]
    Transform viewVisualisationPoint;

    [SerializeField]
    float meshResolution;

    private int edgeResolveIterations = 4;
    private float edgeDistanceThreshold = 0.5f;

    private MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    private bool isTargetInSight;
    public bool IsTargetInSight {get { return isTargetInSight; } }

    private Transform targetInFOW;
    public Transform Target {get { return targetInFOW; } }

    private void Start() 
    {
        viewMeshFilter = viewVisualisationPoint.GetComponent<MeshFilter>();
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    private void Update()
    {
        CheckTargetsInFOW();
    }

    private void LateUpdate() 
    {
        DrawFieldOfView();
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

                if (isTargetInSight)
                {
                    targetInFOW = target;
                }
            }
        }
    }

    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3> ();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = viewVisualisationPoint.eulerAngles.y - viewAngle / 2f + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if ( i > 0)
            {
                bool isEdgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) >  edgeDistanceThreshold;

                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && isEdgeDistanceThresholdExceeded) )
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = viewVisualisationPoint.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;
        
        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2f;
            ViewCastInfo newViewCast = ViewCast(angle);
            bool isEdgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) >  edgeDistanceThreshold;

            if (newViewCast.hit == minViewCast.hit && !isEdgeDistanceThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;
        
        if (Physics.Raycast(viewVisualisationPoint.position, direction, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, viewVisualisationPoint.position + direction * viewRadius, viewRadius, globalAngle);
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
    
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}
