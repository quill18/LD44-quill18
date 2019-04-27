using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
        weaponSlots = GetComponentsInChildren<WeaponSlot>();
    }

    public bool FiresWithSubUnits;

    private void OnEnable()
    {
        if (AllEnemies == null)
            AllEnemies = new List<EnemyUnit>();

        AllEnemies.Add(this);
    }

    private void OnDisable()
    {
        AllEnemies.Remove(this);
    }

    static public List<EnemyUnit> AllEnemies;

    WeaponSlot[] weaponSlots;

    protected Unit unit;

    // Update is called once per frame
    void Update()
    {
        DoAI();
        DoShoot();
    }

    protected virtual void DoAI()
    {
        // VERY simple AI
        unit.Velocity = new Vector3(-unit.MaxSpeed, 0, 0);
    }
    
    protected virtual void DoShoot()
    {
        if (FiresWithSubUnits == false && HasSubUnits())
            return;

        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.Fire();
        }
    }

    protected bool HasSubUnits()
    {
        EnemyUnit[] wss = GetComponentsInChildren<EnemyUnit>();

        if (wss.Length > 1)
            return true;

        return false;
        
    }

}
