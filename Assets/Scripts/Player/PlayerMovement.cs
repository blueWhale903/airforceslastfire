using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector2 _movementDirection;
    private float boundaryOffset = 15;
    private Rigidbody2D _rb2d;
    private Camera _camera;
    private bool isFacingRight = true;
    private SpriteRenderer _sprite;

    public InputAction inputAction;
    public InputActionReference move;
    public PlayerShooting playerShooting;

    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _camera = Camera.main;
    }
    
    void Update()
    {
        _movementDirection = move.action.ReadValue<Vector2>();

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        PreventOutOfScreen();

        _rb2d.linearVelocity = _speed * _movementDirection;
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;

        localScale.x *= -1f;
        playerShooting.SetShootDirection(new Vector2(localScale.x, 0));

        transform.localScale = localScale;
    }
    private void PreventOutOfScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if (screenPosition.x < boundaryOffset && _movementDirection.x < 0 ||
            screenPosition.x > _camera.pixelWidth - boundaryOffset && _movementDirection.x > 0)
        {
            _movementDirection.x = 0;
        }
        if (screenPosition.y < boundaryOffset && _movementDirection.y < 0 ||
            screenPosition.y > _camera.pixelHeight - boundaryOffset && _movementDirection.y > 0)
        {
            _movementDirection.y = 0;
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
    }

}
