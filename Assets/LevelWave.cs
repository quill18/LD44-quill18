using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelWave
{

    public GameObject EnemyPrefab;
    public int NumEnemies = 5;
    public float DelayBetweenEnemies = 0.25f;
    public Transform SpawnLocation;
    public Vector3 SpawnOffset;
    public float DelayAfterWave = 5f;

}
