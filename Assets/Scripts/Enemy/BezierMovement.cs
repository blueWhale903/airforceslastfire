using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class BezierMovement : MonoBehaviour
{
    public BezierParams bezierParams { get; set; }
    public int resolution = 50; // how many points to sample
    private List<float> arcLengths = new List<float>();
    private float totalLength;
    private float distanceTravelled = 0f;

    private Camera _camera => Camera.main;

    void Start()
    {
        if (bezierParams == null) return;

        arcLengths.Clear();
        Vector3 prev = bezierParams.start;
        float length = 0f;
        arcLengths.Add(0f);

        for (int i = 1; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 point = GetPoint(t);
            length += Vector3.Distance(prev, point);
            arcLengths.Add(length);
            prev = point;
        }

        totalLength = length;
        bezierParams.duration = totalLength / 2.5f;
    }

    void Update()
    {
        if (bezierParams == null) return;

        distanceTravelled += (totalLength / bezierParams.duration) * Time.deltaTime;

        if (distanceTravelled > totalLength)
        {
            if (OutOfScreen()) Destroy(this.gameObject);
            distanceTravelled = totalLength;
        }


        float t = GetTForDistance(distanceTravelled);
        transform.position = GetPoint(t);
    }

    private Vector3 GetPoint(float t)
    {
        float u = 1 - t;
        return u * u * bezierParams.start +
               2 * u * t * bezierParams.control +
               t * t * bezierParams.end;
    }

    // Find parameter t that corresponds to a given distance
    private float GetTForDistance(float d)
    {
        for (int i = 1; i < arcLengths.Count; i++)
        {
            if (arcLengths[i] >= d)
            {
                float segmentLen = arcLengths[i] - arcLengths[i - 1];
                float segmentT = (d - arcLengths[i - 1]) / segmentLen;
                return (i - 1 + segmentT) / resolution;
            }
        }
        return 1f;
    }

    public void SetBeziers(BezierParams bp)
    {
        bezierParams = bp;
        Start();
    }

    private bool OutOfScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if (screenPosition.x < 0 || screenPosition.x > _camera.pixelWidth ||
            screenPosition.y < 0 || screenPosition.y > _camera.pixelHeight) 
        {
            return true;
        }

        return false;
    }

    public static float ApproximateCurveLength(Vector3 p0, Vector3 p1, Vector3 p2, int resolution = 50)
    {
        float length = 0f;
        Vector3 previousPoint = p0;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 point = Mathf.Pow(1 - t, 2) * p0 +
                            2 * (1 - t) * t * p1 +
                            Mathf.Pow(t, 2) * p2;
            length += Vector3.Distance(previousPoint, point);
            previousPoint = point;
        }

        return length;
    }
}
