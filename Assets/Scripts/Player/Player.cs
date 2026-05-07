using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float ghostTime = 2;
    [SerializeField] GameObject hurtStar;
    private int health { get; set; }
    private bool isGhost = false;

    private Animator _animator;
    [SerializeField] private CameraShake cameraShake;

    [SerializeField] Image[] heartList;
    [SerializeField] Animator SceneTransition;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        health = 3;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        AudioManager.Instance.PlayPlayerHurtSFX(4.5f);
    }

    private void ShowHearts()
    {
        for (int i = 0; i < heartList.Length; i++)
        {
            heartList[i].enabled = (i+1) < health;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isGhost) return;

        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Enemy") || obj.CompareTag("Bullet"))
        {
            ShowHearts();
            StartCoroutine(GetHit());

            if (obj.CompareTag("Bullet"))
            {
                Destroy(obj);
            }
        }
    }

    private IEnumerator GetHit()
    {
        TakeDamage(1);
        isGhost = true;

        hurtStar.GetComponent<Animator>().Play("player_hurt_star");
        hurtStar.transform.Rotate(new Vector3(0,0,Random.Range(0,360)));
        StartCoroutine(cameraShake.Shake(0.2f, 0.3f));
        
        Time.timeScale = 0.08f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;

        if (health <= 0)
        {
            SceneTransition.Play("SceneTransitionReverse");
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("Level 1");
        }

        _animator.Play("player_hurt");
        hurtStar.GetComponent<Animator>().Play("player_hurt_star_reset");
        yield return new WaitForSeconds(ghostTime);

        _animator.Play("player");

        isGhost = false;
    }
}
