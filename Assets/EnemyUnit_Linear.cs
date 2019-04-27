using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit_Linear : EnemyUnit
{
    public Vector3 Direction;

    protected override void DoAI()
    {
        // VERY simple AI
        unit.Velocity = Direction;
    }
}
