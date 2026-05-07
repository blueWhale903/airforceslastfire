using UnityEngine;

public class SpreadShot : IShootingPattern
{
    private int bulletCount;
    private float spreadAngle;

    public SpreadShot(int bulletCount, float spreadAngle)
    {
        this.bulletCount = bulletCount;
        this.spreadAngle = spreadAngle;
    }

    public void Shoot(Transform firePoint, Transform target, GameObject bulletPrefab)
    {
        float startAngle = -spreadAngle / 2f;
        float step = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = Random.Range(startAngle, spreadAngle);//startAngle + i * step;
            Quaternion rot = firePoint.rotation * Quaternion.Euler(0, 0, angle);
            GameObject bullet = Object.Instantiate(bulletPrefab, firePoint.position, rot);
            Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();

            if (rb2d)
            {
                rb2d.linearVelocity = rot * (target.position - firePoint.position).normalized * 2;
            }
        }
    }
}
