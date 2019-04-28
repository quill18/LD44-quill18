using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit_UpDown : EnemyUnit
{
    public float ScreenBoundLimits = 0.1f;
    float dir = 1;
    float targetX = 6.656474f;

    protected override void DoAI()
    {
        Bounds unitBounds = new Bounds();
        Collider2D[] cols = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in cols)
        {
            unitBounds.Encapsulate(col.bounds);
        }

        if(this.transform.position.x > targetX)
        {
            unit.Velocity = new Vector3(-unit.MaxSpeed, 0, 0);
            return;
        }

        // Is the TOP of our sprite above the top of our screen?
        // Use ScreenBoundLimits as a border/padding
        if (unitBounds.max.y > WorldManager.Instance.ScreenBounds.max.y - ScreenBoundLimits)
        {
            dir = -1;
        }
        else if (unitBounds.min.y < WorldManager.Instance.ScreenBounds.min.y + ScreenBoundLimits)
        {
            dir = 1;
        }

        unit.Velocity = new Vector3(0, unit.MaxSpeed * dir, 0);
    }
}
