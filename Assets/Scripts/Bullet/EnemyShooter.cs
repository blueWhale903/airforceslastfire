using UnityEngine;
public enum ShootingPatternType { Single, Circle, Spread }

public class EnemyShooter : MonoBehaviour
{
    public Transform target;
    public ShootingPatternType shootingPatternType;
    public GameObject bulletPrefab;
    public float shootInterval = 1.5f;

    private IShootingPattern shootingPattern;
    private float timer;

    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        target = player.transform;
    }

    void Start()
    {
        switch (shootingPatternType)
        {
            case ShootingPatternType.Single:
                shootingPattern = new SingleShot();
                break;
            case ShootingPatternType.Spread:
                shootingPattern = new SpreadShot(3, 30);
                break;
            case ShootingPatternType.Circle:
                shootingPattern = new CircleShot(8);
                break;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= shootInterval)
        {
            shootingPattern.Shoot(transform, target, bulletPrefab);
            timer = 0f;
        }
    }

    public void SetShootingPattern(IShootingPattern pattern)
    {
        shootingPattern = pattern;
    }
}
