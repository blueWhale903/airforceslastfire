using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float flashTime; 
    [SerializeField] Color flashColor = Color.white; 

    SpriteRenderer sprite;
    Material material;

    private Coroutine damageFlashCoroutine;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        material = sprite.material;
    }

    void Start()
    {
        setFlashAmount(0);
    }

    public void CallDamageFlash()
    {
        damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        setFlashColor();

        float currentFlashAmount = 0;
        float eslapsedTime = 0;

        while (eslapsedTime < flashTime)
        {
            eslapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1, 0, (eslapsedTime / flashTime));
            setFlashAmount(currentFlashAmount);

            yield return null;
        }

    }

    void setFlashAmount(float amount)
    {
        material.SetFloat("_FlashAmount", amount);
    }

    void setFlashColor()
    {
        material.SetColor("_FlashColor", flashColor);
    }

        
}
