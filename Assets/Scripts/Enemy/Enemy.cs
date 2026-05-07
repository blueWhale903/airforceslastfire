using System;
using UnityEditor.Search;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected int health;
    public event System.Action<Enemy> OnEnemyDie;
    protected string explode_animation = "enemy_explode";
    
    protected DamageFlash damageFlash;

    protected Animator _animator;

    void Start()
    {
        damageFlash = GetComponent<DamageFlash>();
        _animator = GetComponent<Animator>();
        Debug.Log(damageFlash); 
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        AudioManager.Instance.PlayPlayerHurtSFX(1f);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject GO = collision.gameObject;
        IDamager damage_interface = GO.GetComponent<IDamager>();

        if (damage_interface != null && GO.tag == "Player")
        {
            int damage = damage_interface.Damage;
            TakeDamage(damage);
            damageFlash.CallDamageFlash();

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        OnEnemyDie?.Invoke(this);

        GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default")); ;
        GetComponent<Collider2D>().enabled = false;
        _animator.Play(explode_animation);
        float animLength = _animator.GetCurrentAnimatorStateInfo(0).length;
        AudioManager.Instance.PlayEnemyExplodeSFX();
        if (explode_animation == "boss_explode")
        {
            //Destroy(gameObject, 1.8f);
        } else
        {
            Destroy(gameObject, 0.3f);
        }
    }


}
