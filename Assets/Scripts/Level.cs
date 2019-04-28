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
    float levelEndDelay = 2f;

    public void Update()
    {
        if (WorldManager.Instance.IsPaused)
            return;

        waveTimer -= Time.deltaTime;

        if (waveNumber == Waves.Length)
        {
            // We've spawned everything for this level. 


            if(this.transform.childCount <= 0)
            {
                // No more enemies left in scene
                // Maybe show countdown?

                levelEndDelay -= Time.deltaTime;

                if (waveTimer <= 0 && levelEndDelay <= 0)
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
            Waves[waveNumber].SpawnLocation + Waves[waveNumber].SpawnOffset * didSpawn,
            Quaternion.identity);
        ModifyEnemyByLevel(go);
        go.transform.SetParent(this.transform);
        didSpawn++;

        if (didSpawn < Waves[waveNumber].NumEnemies)
            waveTimer = Waves[waveNumber].DelayBetweenEnemies;
        else
            waveTimer = Waves[waveNumber].DelayAfterWave;

    }

    void ModifyEnemyByLevel(GameObject enemyGO)
    {
        int level = WorldManager.Instance.GetCurrentLevel();



        Unit[] us = enemyGO.GetComponentsInChildren<Unit>();

        foreach(Unit u in us)
        {
            u.Health = (int)((float)u.Health * (1 + (float)level / 4f));
            u.Health += (level / 3);

            WeaponSlot ws = enemyGO.GetComponent<WeaponSlot>();
            if (ws != null)
            {
                ws.FireRateBonus = (float)level * 0.05f;
                ws.multishot += (level / 4);

                /*if(ws.multishot > 9)
                {
                    ws.DamageBonus = ws.multishot - 9;
                    ws.multishot = 9;
                }*/
            }
        }
    }

}
