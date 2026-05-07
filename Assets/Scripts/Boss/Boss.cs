using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : Enemy
{
    [SerializeField] GameObject bossHealthBarUI;
    public Slider healthBarSlider;
    public float moveSpeed = 100;
    public GameObject bulletPrefab;
    //private Animator _animator;

    private IShootingPattern spreadPattern;
    private IShootingPattern singlePattern;
    private Transform target;
    private int singleShootCount = 10;

    float timer = 0;
    float shootTimer = 0;
    Vector2 minBounds;
    Vector2 maxBounds;

    private bool active = false;

    public enum BossState { Idle, MovingLeft, MovingRight, MovingUp, MovingDown, Shooting, SwoopDashing }
    public BossState currentState = BossState.Idle;
    private BossState previousBossState;

    void Awake()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        sprite.enabled = false;
        collider.enabled = false;

        damageFlash = GetComponent<DamageFlash>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        explode_animation = "boss_explode";
        healthBarSlider.maxValue = health;
        healthBarSlider.value = health;

        //_animator = GetComponent<Animator>();
        GameObject player = GameObject.Find("Player");
        target = player.transform;

        singlePattern = new SingleShot();
        spreadPattern = new SpreadShot(10, 16);

        Camera mainCam = Camera.main;
        minBounds = mainCam.ViewportToWorldPoint(new Vector3(0.14f, 0.14f, 0));
        maxBounds = mainCam.ViewportToWorldPoint(new Vector3(0.86f, 0.86f, 0));
    }

    private void Update()
    {
        if (!active) return; 

        timer -= Time.deltaTime;
        shootTimer -= Time.deltaTime;

        if (timer <= 0)
        {
            ChooseAction();
        }


        PerformState();
    }

    public IEnumerator Spawn()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        CircleCollider2D collider = GetComponent<CircleCollider2D>();

        Vector3 targetPosition = transform.position; 
        Vector3 flyDirection = new Vector3(9, 22, 0); 
        Vector3 startPosition = targetPosition + flyDirection;

        transform.position = startPosition;
        sprite.color = new Color(1, 1, 1, 0); 
        sprite.enabled = true;
        collider.enabled = false;

        float duration = 4.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / duration;

            float smoothProgress = 1f - Mathf.Pow(1f - progress, 3f);

            transform.position = Vector3.Lerp(startPosition, targetPosition, smoothProgress);

            sprite.color = new Color(1, 1, 1, progress);

            yield return null;
        }

        transform.position = targetPosition;
        sprite.color = Color.white;

        bossHealthBarUI.SetActive(true);

        yield return new WaitForSecondsRealtime(0.5f);

        collider.enabled = true;
        active = true;
    }

    private void ChooseAction()
    {
        previousBossState = currentState;

        timer = Random.Range(1.5f, 4);

        float p;
        if (previousBossState == BossState.Shooting)
        {
            p = Random.Range(0.35f, 0.95f);
        } else
        {
            p = Random.Range(0f, 1f);
        }

        currentState = p switch
        {
            < 0.35f => BossState.Shooting,
            < 0.50f => BossState.MovingLeft,
            < 0.65f => BossState.MovingRight,
            < 0.80f => BossState.MovingDown,
            < 0.95f => BossState.MovingUp,
            _ => BossState.Shooting
        };
    }
    private void PerformState()
    {
        Vector3 currentPos = transform.position;
        moveSpeed = health > healthBarSlider.maxValue / 2.0f ? Random.Range(1.0f, 5.0f) : Random.Range(3.0f, 8.0f);
        float speed = moveSpeed * Time.deltaTime;

        switch (currentState)
        {
            case BossState.MovingUp:
                if (currentPos.y < maxBounds.y)
                    transform.Translate(Vector3.up * speed);
                else ChooseAction(); 
                break;

            case BossState.MovingDown:
                if (currentPos.y > minBounds.y)
                    transform.Translate(Vector3.down * speed);
                else ChooseAction();
                break;

            case BossState.MovingLeft:
                if (currentPos.x > minBounds.x)
                    transform.Translate(Vector3.left * speed);
                else ChooseAction();
                break;

            case BossState.MovingRight:
                if (currentPos.x < maxBounds.x)
                    transform.Translate(Vector3.right * speed);
                else ChooseAction(); 
                break;
            case BossState.Shooting:
                Shoot();
                break;
        }
    }

    private void Shoot()
    {
        if (shootTimer <= 0)
        {

            int randomShooter = Random.Range(0, 2);

            switch (randomShooter)
            {
                case 0:
                    {
                        singlePattern.Shoot(transform, target, bulletPrefab);
                        shootTimer = 0.18f;
                        singleShootCount -= 1;
                        if (singleShootCount <= 0) {
                            ChooseAction();
                            singleShootCount = 10;
                        }
                        break;
                    }
                case 1:
                    {
                        spreadPattern.Shoot(transform, target, bulletPrefab);
                        shootTimer = 1f;
                        ChooseAction();
                        break;
                    }
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            active = false;
            StartCoroutine(Win());
        }

        healthBarSlider.value = health;
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Win");
    }
}
