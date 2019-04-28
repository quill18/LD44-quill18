using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit_Wavy : EnemyUnit
{
    float timer = 0;
    float waveDist = 6;
    float waveFreq = 3;

    protected override void DoAI()
    {
        timer += Time.deltaTime;

        unit.Velocity = new Vector3(-unit.MaxSpeed, Mathf.Sin(timer * waveFreq) * waveDist, 0);
    }
}
