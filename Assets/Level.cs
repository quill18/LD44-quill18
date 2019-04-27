using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public void Start()
    {
    }

    public LevelWave[] Waves;
    int waveNumber = 0;
    float waveTimer = 0;
    int didSpawn = 0;

    public void Update()
    {
        waveTimer -= Time.deltaTime;

        if (waveNumber == Waves.Length)
        {
            // We've spawned everything for this level. 


            if(this.transform.childCount <= 0)
            {
                // No more enemies left in scene
                // Maybe show countdown?

                if(waveTimer <= 0)
                {
                    // Show shopping screen and get ready for next level.
                    WorldManager.Instance.EndLevel();
                }

            }

            return;
        }

        // Do we still have to spawn dudes?
        bool spawned = false;
        while (didSpawn < Waves[waveNumber].NumEnemies && waveTimer <= 0)
        {
            SpawnNext();
            spawned = true;
        }

        if (spawned)
            return;
        
        // If we get here, we're done spawning, waiting for delay
        if(waveTimer <= 0)
        {
            // Next wave!
            waveNumber++;
            didSpawn = 0;

            if(waveNumber == Waves.Length)
            {
                // No more waves left -- give the player 5 seconds to enjoy / pick up scrap
                waveTimer = 5;
            }
        }
        
    }

    void SpawnNext()
    {
        GameObject go = Instantiate(Waves[waveNumber].EnemyPrefab,
            Waves[waveNumber].SpawnLocation.position + Waves[waveNumber].SpawnOffset * didSpawn,
            Quaternion.identity);
        go.transform.SetParent(this.transform);
        didSpawn++;

        if (didSpawn < Waves[waveNumber].NumEnemies)
            waveTimer = Waves[waveNumber].DelayBetweenEnemies;
        else
            waveTimer = Waves[waveNumber].DelayAfterWave;

    }

}
