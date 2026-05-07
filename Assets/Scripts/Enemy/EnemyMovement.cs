using UnityEngine;
using UnityEngine.U2D;

public class EnemyMovement : MonoBehaviour
{
    private MovementParams movementParams;
    private Vector3 startPos;
    private float t = 0f;
    private float distanceTravelled = 0f;
    private GameObject target;
    private Vector3 direction;
    private Camera _camera => Camera.main;

    private void Start()
    {
        target = GameObject.Find("Player");
    }

    public void SetMovement(MovementParams p_movementParams)
    {
        this.movementParams = p_movementParams;
        startPos = transform.position;
        t = 0f;
        distanceTravelled = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementParams == null) return;
        
        t += Time.deltaTime;

        switch (movementParams.type)
        {
            case MovementType.Bezier:
                MoveBezier();
                break;
            case MovementType.Sine:
                MoveSine();
                break;
            case MovementType.Follow:
                MoveFollow();
                break;
            case MovementType.Random:
                MoveRandom();
                break;
        }
    }

    void MoveBezier()
    {
        distanceTravelled += movementParams.speed * Time.deltaTime;

        float t = distanceTravelled / movementParams.bezier.curveLength;
        t = Mathf.Clamp01(t);
        Vector3 pos =
            Mathf.Pow(1 - t, 2) * movementParams.bezier.start +
            2 * (1 - t) * t * movementParams.bezier.control +
            Mathf.Pow(t, 2) * movementParams.bezier.end;

        transform.position = pos;
    }

    void MoveSine()
    {
        transform.position += new Vector3(-Mathf.Sign(startPos.x),0,0) * movementParams.speed * Time.deltaTime;
        transform.position = new Vector3(
            transform.position.x,
            startPos.y + Mathf.Sin(t * movementParams.frequency) * movementParams.amplitude,
            0
        );
    }

    //void MoveCircle()
    //{
    //    float angle = Time.time * movementParams.speed;
    //    transform.position = movementParams.circleCenter + new Vector3(Mathf.Cos(angle) * movementParams.radius, Mathf.Sin(angle) * movementParams.radius, 0);
    //}
    
    void MoveFollow()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += movementParams.speed * Time.deltaTime * direction;
    }

    void MoveRandom()
    {
        if (t >= Random.Range(3, 6) || OutOfScreen())
        {
            float theta = Random.Range(0, 2 * Mathf.PI);
            float radius = Random.Range(0f, movementParams.radius);
            Vector3 newPos = new Vector3(Mathf.Cos(theta) * radius, Mathf.Sin(theta) * radius, 0f);
            direction = (newPos - transform.position).normalized;
            t = 0f;
        }

        transform.position += movementParams.speed * Time.deltaTime * direction;
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
}
