using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();

        unit.LimitedToScreenBounds = true;

        weaponSlots = GetComponentsInChildren<WeaponSlot>();

        scrapShield = GetComponentInChildren<ScrapShield>();
    }

    Unit unit;
    WeaponSlot[] weaponSlots;
    public float PickupRange = 0.5f;
    ScrapShield scrapShield;

    public int MaxMegabombs = 0;
    public int CurrentMegabombs = 0;
    public int MegabombDamage = 3;

    public int MaxBullettime = 0;
    public int CurrentBullettime = 0;

    public int MaxDrones = 0;
    public int DroneArmor = 0;
    public int DroneFireRateBonus = 0;
    public int DroneDamageBonus = 0;

    public GameObject AttackDronePrefab;

    GameObject[] activeDrones;

    int scrapLimit = 50; // Above this you start to get encumbered.

    static bool firstScrap = true;

    public void PickupScrap( Scrap scrap )
    {
        if(firstScrap)
        {
            firstScrap = false;
            WorldManager.Instance.Tutorial_Scrap();
        }

        WorldManager.Instance.ScrapCollected++;

        unit.GainHealth(1);
        scrapShield.GainScrap(scrap);
        SoundManager.Instance.PlaySlurp();
    }

    public int ScrapCount()
    {
        if (scrapShield == null)
            return 0;
        return scrapShield.ScrapCount();
    }

    public bool OverEncumbered()
    {
        return ScrapCount() > scrapLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (WorldManager.Instance.IsPaused)
            return;

        float speed = unit.MaxSpeed;

        int scrapCount = ScrapCount();

        if (OverEncumbered())
        {
            int overage = scrapCount - scrapLimit;
            speed = Mathf.Lerp(speed, speed/4f, (float)overage / (float)scrapLimit);
        }

        unit.Velocity.x = Input.GetAxis("Horizontal");
        unit.Velocity.y = Input.GetAxis("Vertical");
        unit.Velocity = unit.Velocity.normalized *speed;

        if (Input.GetButton("Fire"))
        {
            foreach (WeaponSlot ws in weaponSlots)
            {
                ws.Fire();
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            LaunchMegabomb();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            LaunchbBullettime();
        }
    }

    public void ResetForNewLevel()
    {
        CurrentMegabombs = MaxMegabombs;
        CurrentBullettime = MaxBullettime;

        if(activeDrones != null)
        {
            for (int i = 0; i < activeDrones.Length; i++)
            {
                if(activeDrones[i] != null)
                    activeDrones[i].GetComponent<Unit>().Die(true);
            }
        }

        activeDrones = new GameObject[MaxDrones];
        for (int i = 0; i < activeDrones.Length; i++)
        {
            activeDrones[0] = Instantiate(AttackDronePrefab, this.transform.position, Quaternion.identity);
            Unit droneUnit = activeDrones[0].GetComponent<Unit>();
            droneUnit.GetComponent<DroneUnit>().ParentPlayer = this;
            droneUnit.Health = DroneArmor;
            droneUnit.DamageBonus = DroneDamageBonus;
            droneUnit.Set_FireRateBonus(DroneFireRateBonus);
        }
    }

    public void LaunchMegabomb()
    {
        // TODO: FX

        if (CurrentMegabombs <= 0)
            return;

        CurrentMegabombs--;

        EnemyUnit[] eus = EnemyUnit.AllEnemies.ToArray();
        for (int i = 0; i < eus.Length; i++)
        {
            eus[i].GetComponent<Unit>().TakeDamage(MegabombDamage);
        }

        WorldManager.Instance.FX_MegaBomb();
    }

    public void LaunchbBullettime()
    {
        // Do Post FX to desaturate and maybe add a sort of TV/tracking thing? Or more grainy/pixelate -- like old CRT?
        // Post FX for matrix
        // Tint green?


        if (CurrentBullettime <= 0)
            return;

        CurrentBullettime--;

        WorldManager.Instance.StartBulletTime(5);
    }

    public void Upgrade_FireRate(float amt)
    {
        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.FireRateBonus += amt;
        }

    }

    public void Set_FireRate(float amt)
    {
        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.FireRateBonus = amt;
        }

    }

    public void Upgrade_ScrapRange()
    {
        PickupRange += 1;
    }

    public void Upgrade_MultiShot()
    {
        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.multishot += 1;
        }

    }

    public void Upgrade_Damage()
    {
        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.DamageBonus += 1;
        }

    }

    public void Set_BonusDamage(int amt)
    {
        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.DamageBonus = amt;
        }

    }


    public void Upgrade_Piercing( float amt )
    {
        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.PierceChance += amt;
        }
    }

    public void Upgrade_Homing( )
    {
        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.HomingTurnRate += 15;
        }
    }

    public void Upgrade_CritChance(float amt)
    {
        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.CritChance += amt;
        }
    }

    public void Upgrade_Engines(int amt)
    {
        scrapLimit += amt;
    }

    public void Upgrade_Invulnerability()
    {
        unit.InvulnerabilityDuration += 0.10f;
    }

    
    public void Upgrade_BulletSpeed(float amt)
    {
        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.SpeedBonus += amt;
        }
    }

    public void Improved_HeatSinks()
    {
        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.HeatPerShot *= 0.80f;
            //ws.HeatSink *= 1.05f;
        }

    }

    public void Upgrade_Megabombs()
    {
        MaxMegabombs++;
    }

    public void Upgrade_MegabombDamage()
    {
        MegabombDamage += 2;
    }

    public void Upgrade_Bullettime()
    {
        MaxBullettime++;
    }

    public void Upgrade_AddDrone()
    {
        MaxDrones++;
    }

    public void Upgrade_DroneArmor()
    {
        DroneArmor += 3;
    }

    public void Upgrade_DroneDamage()
    {
        DroneDamageBonus += 2;
    }

    public void Upgrade_DroneFireRate()
    {
        DroneFireRateBonus++;
    }
}
