using UnityEngine;

public class AutoScroll : MonoBehaviour
{
    [Header("Movement Settings")]
    public float scrollSpeed = 2f;

    private float width;

    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log(width);
    }

    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (transform.position.x <= -width)
        {
            Vector2 resetPosition = new Vector2(width * 2f, 0);
            transform.position = (Vector2)transform.position + resetPosition;
        }
    }
}