using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IDamager
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 1;
    public int Damage => damage;

    private Vector2 direction = Vector2.right;
    private PlayerShooting pool;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool isExploding = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        if (_spriteRenderer != null)
            _spriteRenderer.flipX = direction.x < 0;
        isExploding = false;
    }

    public void SetPool(PlayerShooting shooter)
    {
        pool = shooter;
    }

    void Update()
    {
        if (!isExploding)
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnBecameInvisible()
    {
        if (!isExploding && pool != null)
            pool.ReturnBulletToPool(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExploding) return;

        if (collision.CompareTag("Enemy"))
        {
            isExploding = true;
            float randomScale = Random.Range(0.5f, 1f);
            Vector2 offset = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
            transform.Translate(offset);
            transform.localScale = new Vector3(randomScale, randomScale, 1f);

            if (_animator != null)
            {
                _animator.Play("player_bullet_explode");
                StartCoroutine(ReturnToPoolAfterAnimation());
            }
            else
            {
                ReturnToPool();
            }
        }
    }

    private IEnumerator ReturnToPoolAfterAnimation()
    {
        float animLength = 0.2f;
        if (_animator != null)
            animLength = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (pool != null)
            pool.ReturnBulletToPool(gameObject);
    }
}
