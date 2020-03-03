using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FlightPathConstraint : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    [Space]
    public float softConstraintDistance = 5;
    public float hardConstraintDistance = 10;
    [Header("DEBUG")]
    public bool DEBUG_DrawArea;
    public int numOfLinesPerCylinder = 8;
    public float TubeLength = 500;

    private Vector3 pathNormal => (endPosition.transform.position - startPosition.transform.position).normalized; // points from startPosition to endPosition

    private Rigidbody rbToConstrain;

    void Start() {
        rbToConstrain = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        AddForceTowardsCenter();
    }

    public void AddForceTowardsCenter() {
        float distance = GetDistanceFromPathCenter(rbToConstrain.transform.position);
        float forceRatio = Mathf.InverseLerp(softConstraintDistance, hardConstraintDistance + Mathf.Epsilon, distance);
        //Debug.Log("Distance = " + distance + "\t\tForceRatio = " + forceRatio);
        Vector3 correctiveVector = -GetVectorFromPathCenter(rbToConstrain.transform.position);
        Debug.DrawLine(rbToConstrain.transform.position, rbToConstrain.transform.position + correctiveVector);
        rbToConstrain.velocity = Vector3.Lerp(rbToConstrain.velocity.normalized, correctiveVector.normalized, forceRatio).normalized * rbToConstrain.velocity.magnitude;
    }

    float GetDistanceFromPathCenter(Vector3 point) {
        return GetVectorFromPathCenter(point).magnitude;
    }

    Vector3 GetVectorFromPathCenter(Vector3 point) {
        Vector3 startToPointVec = point - startPosition.transform.position;
        return Vector3.ProjectOnPlane(startToPointVec, pathNormal);
    }


    #region UnityEvent-Accesible Functions

    public void SetEndpoint(Transform _transform) {
        endPosition = _transform;
    }
    public void SetStartPoint(Transform _transform) {
        startPosition = _transform;
    }

    public void SetSoftConstraint(float val) {
        softConstraintDistance = val;
    }

    public void SetHardConstraint(float val) {
        hardConstraintDistance = val;
    }

    #endregion


    #region Debug Visuals
    private void OnDrawGizmos() {
        if (!DEBUG_DrawArea) { return; }

        Gizmos.color = Color.cyan;
        Debug.DrawRay(startPosition.transform.position, pathNormal * TubeLength, Gizmos.color, 0, true);
        Gizmos.color = Color.green;

        Vector3 pathRight = Vector3.Cross(pathNormal.normalized, Vector3.up).normalized;
        Vector3 pathUp = Vector3.Cross(pathRight, pathNormal);

        Vector3 drawPosition = startPosition.transform.position + pathUp * softConstraintDistance;
        
        for (int i = 0; i < numOfLinesPerCylinder && numOfLinesPerCylinder >= 0; i++) {
            drawPosition = Quaternion.AngleAxis(360.0f / numOfLinesPerCylinder, pathNormal) * drawPosition;
            Debug.DrawRay(drawPosition, pathNormal * TubeLength, Gizmos.color, 0, true);
        }

        Gizmos.color = Color.red;
        drawPosition = startPosition.transform.position + pathUp * hardConstraintDistance;
        for (int i = 0; i < numOfLinesPerCylinder && numOfLinesPerCylinder >= 0; i++) {
            drawPosition = Quaternion.AngleAxis(360.0f / numOfLinesPerCylinder, pathNormal) * drawPosition;
            Debug.DrawRay(drawPosition, pathNormal * TubeLength, Gizmos.color, 0, true);
        }
    }
    #endregion
}
