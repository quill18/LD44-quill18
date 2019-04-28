using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneUnit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
        weaponSlots = GetComponentsInChildren<WeaponSlot>();
    }

    private void OnEnable()
    {
        targetPositionOffset = Vector3.zero;
    }

    private void OnDisable()
    {
    }

    WeaponSlot[] weaponSlots;

    protected Unit unit;

    public PlayerUnit ParentPlayer;
    Vector3 targetPositionOffset;
    Vector3 currentVelocity;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ParentPlayer == null)
        {
            //Debug.LogError("Drone without a player.");
            return;
        }

        DoAI();
        DoShoot();
    }

    protected virtual void DoAI()
    {
        // Drones pick a spot near the player, SmoothDamp there, then pick another spot, etc...
        if (Vector3.Distance(ParentPlayer.transform.position + targetPositionOffset, this.transform.position) <= 0.5f )
        {
            targetPositionOffset = (Vector3)Random.insideUnitCircle * 2f;
        }

        GetComponent<Rigidbody2D>().position = Vector3.SmoothDamp(transform.position, ParentPlayer.transform.position + targetPositionOffset, ref currentVelocity, .5f);

    }
    
    protected virtual void DoShoot()
    {
        if (EnemyUnit.AllEnemies==null || EnemyUnit.AllEnemies.Count == 0)
            return; // Don't fire if no enemies...it looks stupid.

        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.Fire();
        }
    }
}
