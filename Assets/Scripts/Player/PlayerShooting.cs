using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shootRate = 0.2f;
    [SerializeField] int poolSize = 20;

    private Vector2 shootDirection;
    private Queue<GameObject> bulletPool;

    void Start()
    {
        shootDirection = new Vector2(1, 0);
        InitializePool();
        StartCoroutine(Shoot());
    }

    private void InitializePool()
    {
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullet.GetComponent<PlayerBullet>().SetPool(this);
            bulletPool.Enqueue(bullet);
        }
    }

    public void SetShootDirection(Vector2 direction)
    {
        shootDirection = direction;
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            GameObject bullet = GetBulletFromPool();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);

            PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
            if (playerBullet) playerBullet.SetDirection(shootDirection);

            yield return new WaitForSeconds(shootRate);
        }
    }

    private GameObject GetBulletFromPool()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            return bullet;
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<PlayerBullet>().SetPool(this);
            return bullet;
        }
    }

    // Called by PlayerBullet when bullet is no longer needed
    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
