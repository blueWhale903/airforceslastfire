using UnityEngine;

public class SingleShot : IShootingPattern
{
    public void Shoot(Transform firePoint, Transform target, GameObject bulletPrefab)
    {
        Vector3 offset = new Vector3(Random.Range(-0.65f, 0.65f), Random.Range(-0.65f, 0.65f), 0);
        GameObject bullet = Object.Instantiate(bulletPrefab, firePoint.position + offset, firePoint.rotation);
        Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();

        if (rb2d)
        {
            rb2d.linearVelocity = (target.position - firePoint.position).normalized * 2;
        }
    }
}
