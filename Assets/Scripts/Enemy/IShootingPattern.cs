using UnityEngine;

public interface IShootingPattern
{
    void Shoot(Transform firePoint, Transform target, GameObject bulletPrefab);
}
