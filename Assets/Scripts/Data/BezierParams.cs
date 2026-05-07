using UnityEngine;

[System.Serializable]
public class BezierParams
{
    public Vector3 start;
    public Vector3 control;
    public Vector3 end;
    public float duration;
    public float curveLength => BezierMovement.ApproximateCurveLength(start, control, end);
}
