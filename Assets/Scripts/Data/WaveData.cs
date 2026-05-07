using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveData
{
    public float spawnTime;
    public GameObject enemyPrefab;     // which enemy to spawn
    public Vector3 startPos;
    public List<MovementPhase> phases;  // movement curve
    public int count = 1;              // how many enemies in this wave
    public float spacing = 0.5f;       // time offset between spawns in wave
    public bool lockWave = false;
    public bool spawned = false;
}