using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Settings")]
    public Transform target;
    public List<WaveData> waves;
    [SerializeField] GameObject bossObj;
    [SerializeField] GameObject bossWarningObj;

    private float timer;
    private int lockWaveEnemyCount;
    private bool isLocked = false;
    private int currentWaveIndex = 0;

    void Start()
    {
        SpawnBoss();
        waves = waves.OrderBy(w => w.spawnTime).ToList();
    }

    void Update()
    {
        return;
        if (isLocked) return;

        timer += Time.deltaTime;

        if (currentWaveIndex < waves.Count)
        {
            WaveData nextWave = waves[currentWaveIndex];

            if (nextWave.spawnTime <= timer)
            {
                Debug.Log("Wave: " + (currentWaveIndex));
                StartCoroutine(SpawnWave(nextWave));
                currentWaveIndex++;
            }
        }
    }
    
    private IEnumerator SpawnWave(WaveData wave)
    {
        isLocked = wave.lockWave;

        for (int i = 0; i < wave.count; i++)
        {
            GameObject enemy = SetUpNewEnemy(wave);

            if (wave.spacing > 0)
            {
                yield return new WaitForSeconds(wave.spacing);
            }
        }
    }

    private GameObject SetUpNewEnemy(WaveData wave)
    {
        var enemy = Instantiate(wave.enemyPrefab, wave.startPos, Quaternion.identity);

        var mover = enemy.GetComponent<EnemyMovementSequence>();
        mover.phases = wave.phases;

        var shooter = enemy.GetComponent<EnemyShooter>();
        if (shooter != null)
        {
            shooter.target = target;
        }

        if (wave.lockWave && enemy.TryGetComponent(out Enemy e))
        {
            lockWaveEnemyCount++;
            e.OnEnemyDie += HandleEnemyDeath;
        }
        
        return enemy;
    }


    private void HandleEnemyDeath(Enemy enemy)
    {
        enemy.OnEnemyDie -= HandleEnemyDeath;

        lockWaveEnemyCount--;

        if (lockWaveEnemyCount <= 0)
        {
            lockWaveEnemyCount = 0;
            isLocked = false;

            if (currentWaveIndex >= waves.Count - 1)
            {
                SpawnBoss();
            }
        }
    }

    private void SpawnBoss()
    {
        Boss boss = bossObj.GetComponent<Boss>();

        bossWarningObj.SetActive(true);
        AudioManager.Instance.PlayBossWarningSFX(0.8f);
        StartCoroutine(boss.Spawn());
    }
}
