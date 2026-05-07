using UnityEngine;

public enum MovementType { Bezier, Sine, Random, Follow }

[System.Serializable]
public class MovementParams
{
    public MovementType type;
    public float speed;

    // For Bezier
    public BezierParams bezier;

    // For Sine
    public float amplitude;
    public float frequency;

    // For Random
    public float radius;
}

[System.Serializable]
public class MovementPhase
{
    public MovementParams movementParams;
    public float duration;
    public float waitAfter;
}


