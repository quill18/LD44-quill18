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
        targetPosition = this.transform.position;
    }

    private void OnDisable()
    {
    }

    WeaponSlot[] weaponSlots;

    protected Unit unit;

    public PlayerUnit ParentPlayer;
    Vector3 targetPosition;
    Vector3 currentVelocity;

    // Update is called once per frame
    void Update()
    {
        if(ParentPlayer == null)
        {
            Debug.LogError("Drone without a player.");
            return;
        }

        DoAI();
        DoShoot();
    }

    protected virtual void DoAI()
    {
        // Drones pick a spot near the player, SmoothDamp there, then pick another spot, etc...
        if (Vector3.Distance(targetPosition, this.transform.position) <= 0.1f )
        {
            targetPosition = ParentPlayer.transform.position + (Vector3)Random.insideUnitCircle * 2f;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f);

        //unit.Velocity = new Vector3(-unit.MaxSpeed, 0, 0);
    }
    
    protected virtual void DoShoot()
    {
        if (EnemyUnit.AllEnemies.Count == 0)
            return; // Don't fire if no enemies...it looks stupid.

        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.Fire();
        }
    }
}
