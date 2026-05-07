using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHelth = 500;
    private int health;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHelth;        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            
        } else
        {

        }
    }

    void Die()
    {

    }

    void CheckPhase()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
