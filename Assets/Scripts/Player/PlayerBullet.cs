using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IDamager
{
    private Rigidbody2D _rb2d;
    private Vector2 direction { get; set; }

    [SerializeField] private float speed = 20;
    private float lifeTime = 1;

    [SerializeField] private int damage = 1;
    public int Damage => damage;

    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
        _rb2d.linearVelocity = new Vector2(speed, 0) * direction;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject GO = collision.gameObject;

        if (GO.tag == "Enemy")
        {
            float randomScale = Random.Range(0.5f, 1f);
            Vector2 offset = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
            transform.Translate(offset);
            transform.localScale = new Vector3(randomScale, randomScale, 1f);
            _animator.Play("player_bullet_explode");
            float animLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            _rb2d.linearVelocity = Vector2.zero;
            Destroy(gameObject, animLength);
        }
    }
}
