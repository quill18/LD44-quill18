using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelWave
{

    public GameObject EnemyPrefab;
    public int NumEnemies;
    public float DelayBetweenEnemies;
    public Vector3 SpawnLocation;
    public Vector3 SpawnOffset;
    public float DelayAfterWave;

}
