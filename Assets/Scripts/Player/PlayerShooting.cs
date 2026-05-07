using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shootRate;
    private Vector2 shootDirection;

    void Start()
    {
        shootDirection = new Vector2(1, 0);
        StartCoroutine(Shoot());
    }

    public void SetShootDirection(Vector2 direction)
    {
        shootDirection = direction;
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

            PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
            if (playerBullet) playerBullet.SetDirection(shootDirection);

            yield return new WaitForSeconds(shootRate);
        }
    }
}
