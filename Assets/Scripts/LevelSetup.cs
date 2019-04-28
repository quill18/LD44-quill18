using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    // Start is called before the first frame update
    public void InitLevels()
    {
        spawnSpots = new Dictionary<string, Vector3>();
        GameObject ss = GameObject.Find("SpawnSpots");
        for (int i = 0; i < ss.transform.childCount; i++)
        {
            Transform t = ss.transform.GetChild(i);
            spawnSpots[t.name] = t.position;
        }

        Level1();
        Level1();
        Level1();
    }

    public GameObject Scrap;

    public GameObject EnemyUnit_DiagDownward;
    public GameObject EnemyUnit_DiagUpward;
    public GameObject EnemyUnit_StraightForward;
    public GameObject EnemyUnit_Wavy;

    public GameObject Boss01;

    Dictionary<string, Vector3> spawnSpots;

    Vector3 SpawnSpot(string s)
    {
        if(spawnSpots.ContainsKey(s) == false)
        {
            Debug.LogError("No spawn spot: " + s);
            return Vector3.zero;
        }
        return spawnSpots[s];
    }

    void Level1()
    {
        GameObject go = new GameObject("Level 1");
        go.transform.SetParent(this.transform);
        go.SetActive(false);
        Level l = go.AddComponent<Level>();

        l.Waves = new LevelWave[]
        {
            new LevelWave() {
                EnemyPrefab = Scrap,
                NumEnemies = 2,
                DelayBetweenEnemies = .5f,
                SpawnLocation = SpawnSpot("Front") + Vector3.up,
                SpawnOffset = Vector3.down*2f,
                DelayAfterWave = 4
            },


            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front") + Vector3.up*2,
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front+1") + Vector3.up*2,
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front-1") + Vector3.up*2,
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            /////
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagDownward,
                NumEnemies = 5,
                DelayBetweenEnemies = .25f,
                SpawnLocation = SpawnSpot("Top+1"),
                SpawnOffset = Vector3.zero,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagUpward,
                NumEnemies = 5,
                DelayBetweenEnemies = .25f,
                SpawnLocation = SpawnSpot("Bottom+1"),
                SpawnOffset = Vector3.zero,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_Wavy,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front") + Vector3.up*2,
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagDownward,
                NumEnemies = 5,
                DelayBetweenEnemies = .25f,
                SpawnLocation = SpawnSpot("Top+1"),
                SpawnOffset = Vector3.zero,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_Wavy,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front-1") + Vector3.up*2,
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagDownward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Top+1"),
                SpawnOffset = new Vector3(.75f, .75f, 0),
                DelayAfterWave = 0
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagUpward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0f,
                SpawnLocation = SpawnSpot("Bottom+1"),
                SpawnOffset = new Vector3(.75f, -.75f, 0),
                DelayAfterWave = 2
            },
        };
    }

}
