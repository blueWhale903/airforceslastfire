using System.Collections;
using UnityEngine;

public class LaserShooting : MonoBehaviour
{
    public float shootingSpeed;
    public GameObject laser;

    float shootInterval;
    
    GameObject target;
    Animator laserAnimator;

    void Start()
    {
        target = GameObject.Find("Player");
        shootInterval = 1.0f / shootingSpeed;
        BoxCollider2D laser_bc = laser.GetComponent<BoxCollider2D>();
        SpriteRenderer laser_sprite = laser.GetComponent<SpriteRenderer>();
        laserAnimator = laser.GetComponent<Animator>();

        if (laser_bc)
        {
            laser_bc.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        shootInterval -= Time.deltaTime;

        if (shootInterval <= 0)
        {
            LookAtTarget();
            Shoot();
            shootInterval = 1.0f / Random.Range(shootingSpeed, shootingSpeed*3.0f);
        }
    }

    void LookAtTarget()
    {
        Vector2 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate smoothly
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = targetRotation;
    }

    void Shoot()
    {
        laserAnimator.SetTrigger("Shoot");
    }

    //IEnumerator ShootLoop()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(shootInterval);

    //        // Trigger shoot animation
    //        isShooting = true;
    //        animator.SetTrigger("Shoot");

    //        // Wait until animation finishes
    //        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

    //        isShooting = false;
    //    }
    //}
}
