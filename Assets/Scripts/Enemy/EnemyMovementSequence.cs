using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyMovementSequence : MonoBehaviour
{
    public List<MovementPhase> phases;
    private int currentPhase = 0;
    private float phaseTimer = 0f;
    private EnemyMovement movement;

    void Start()
    {
        movement = GetComponent<EnemyMovement>();
        if (phases.Count > 0)
            movement.SetMovement(phases[0].movementParams);
    }

    void Update()
    {
        if (currentPhase >= phases.Count) return;

        phaseTimer += Time.deltaTime;

        if (phaseTimer >= phases[currentPhase].duration)
        {
            StartCoroutine(NextPhase());
        }
    }

    private IEnumerator NextPhase()
    {
        float wait = phases[currentPhase].waitAfter;
        yield return new WaitForSeconds(wait);

        currentPhase++;
        if (currentPhase < phases.Count)
        {
            movement.SetMovement(phases[currentPhase].movementParams);
            phaseTimer = 0f;
        }
    }
}
