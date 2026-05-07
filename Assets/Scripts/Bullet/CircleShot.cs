using UnityEngine;
using UnityEngine.Rendering;

public class CircleShot : IShootingPattern
{
    public int bulletCount = 8;

    public CircleShot(int bulletCount)
    {
        this.bulletCount = bulletCount;
    }

    public void Shoot(Transform firePoint, Transform target, GameObject bulletPrefab)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (360 / bulletCount);
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
