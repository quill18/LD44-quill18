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

        Level0();
        Level1();
        Level2();
        Level3();
        Level1();

        Level2();
        Level3();
        Level1();
        Level2();
        Level3();

        Level1();
        Level2();
        Level3();
        Level1();
        Level2();

        Level3();
        Level2();
        Level4();
        //LevelJustBoss();

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

    void Level0()
    {
        GameObject go = new GameObject("Level 0");
        go.transform.SetParent(this.transform);
        go.SetActive(false);
        Level l = go.AddComponent<Level>();

        l.Waves = new LevelWave[]
        {
            new LevelWave() {
                EnemyPrefab = Scrap,
                NumEnemies = 4,
                DelayBetweenEnemies = .5f,
                SpawnLocation = SpawnSpot("Front") + Vector3.up,
                SpawnOffset = Vector3.down*2f,
                DelayAfterWave = 8
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 2,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front+1") + Vector3.up*1,
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 2,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front-1") + Vector3.up*1,
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
        };
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
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front+1") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front-1") + Vector3.up*(Random.Range(0f,3f)),
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
                SpawnLocation = SpawnSpot("Front") + Vector3.up*(Random.Range(0f,3f)),
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
                SpawnLocation = SpawnSpot("Front-1") + Vector3.up*(Random.Range(0f,3f)),
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

    void Level2()
    {
        GameObject go = new GameObject("Level 2");
        go.transform.SetParent(this.transform);
        go.SetActive(false);
        Level l = go.AddComponent<Level>();
        l.Waves = new LevelWave[]
        {
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_Wavy,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_Wavy,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front+1") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_Wavy,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front-1") + Vector3.up*(Random.Range(0f,3f)),
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
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front-1") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front+1") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagUpward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0f,
                SpawnLocation = SpawnSpot("Bottom+2"),
                SpawnOffset = new Vector3(.75f, -.75f, 0),
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagUpward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0f,
                SpawnLocation = SpawnSpot("Bottom+1"),
                SpawnOffset = new Vector3(.75f, -.75f, 0),
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagUpward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0f,
                SpawnLocation = SpawnSpot("Bottom"),
                SpawnOffset = new Vector3(.75f, -.75f, 0),
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagDownward,
                NumEnemies = 5,
                DelayBetweenEnemies = .25f,
                SpawnLocation = SpawnSpot("Top+2"),
                SpawnOffset = Vector3.zero,
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
                EnemyPrefab = EnemyUnit_DiagDownward,
                NumEnemies = 5,
                DelayBetweenEnemies = .25f,
                SpawnLocation = SpawnSpot("Top"),
                SpawnOffset = Vector3.zero,
                DelayAfterWave = 2
            },

        };
    }

    void Level3()
    {
        GameObject go = new GameObject("Level 3");
        go.transform.SetParent(this.transform);
        go.SetActive(false);
        Level l = go.AddComponent<Level>();
        l.Waves = new LevelWave[]
        {
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 1
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagDownward,
                NumEnemies = 5,
                DelayBetweenEnemies = .25f,
                SpawnLocation = SpawnSpot("Top+1"),
                SpawnOffset = Vector3.zero,
                DelayAfterWave = 1
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagUpward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0f,
                SpawnLocation = SpawnSpot("Bottom+1"),
                SpawnOffset = new Vector3(.75f, -.75f, 0),
                DelayAfterWave = 3
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_Wavy,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front-1") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 3
            },
            new LevelWave() {
                EnemyPrefab = Boss01,
                NumEnemies = 1,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front"),
                SpawnOffset = Vector3.zero,
                DelayAfterWave = 0
            },

        };
    }

    void Level4()
    {
        GameObject go = new GameObject("Level 4");
        go.transform.SetParent(this.transform);
        go.SetActive(false);
        Level l = go.AddComponent<Level>();
        l.Waves = new LevelWave[]
        {
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 2
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_StraightForward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 1
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagDownward,
                NumEnemies = 5,
                DelayBetweenEnemies = .25f,
                SpawnLocation = SpawnSpot("Top+1"),
                SpawnOffset = Vector3.zero,
                DelayAfterWave = 1
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_DiagUpward,
                NumEnemies = 5,
                DelayBetweenEnemies = 0f,
                SpawnLocation = SpawnSpot("Bottom+1"),
                SpawnOffset = new Vector3(.75f, -.75f, 0),
                DelayAfterWave = 3
            },
            new LevelWave() {
                EnemyPrefab = EnemyUnit_Wavy,
                NumEnemies = 5,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front-1") + Vector3.up*(Random.Range(0f,3f)),
                SpawnOffset = Vector3.down,
                DelayAfterWave = 3
            },
            new LevelWave() {
                EnemyPrefab = Boss01,
                NumEnemies = 2,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front") + Vector3.up * 2f,
                SpawnOffset = Vector3.down * 4f,
                DelayAfterWave = 0
            },

        };
    }

    void LevelJustBoss()
    {
        GameObject go = new GameObject("Level 4");
        go.transform.SetParent(this.transform);
        go.SetActive(false);
        Level l = go.AddComponent<Level>();
        l.Waves = new LevelWave[]
        {
            new LevelWave() {
                EnemyPrefab = Boss01,
                NumEnemies = 2,
                DelayBetweenEnemies = 0,
                SpawnLocation = SpawnSpot("Front") + Vector3.up * 2f,
                SpawnOffset = Vector3.down * 4f,
                DelayAfterWave = 0
            },
        };
    }
}
