using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Upgrades = new List<Upgrade>();

        Upgrades.Add(new Upgrade("Fire Rate", "Increase the fire rate of your primary weapon by 10%", 1, (p) => { p.Upgrade_FireRate( 0.1f ); }));
        Upgrades.Add(new Upgrade("Multi Shot", "Increases the number of bullets you fire", 1, (p) => { p.Upgrade_MultiShot(); }));
        Upgrades.Add(new Upgrade("Damage", "Increases the base damage of your bullets", 1, (p) => { p.Upgrade_Damage(); }));
        Upgrades.Add(new Upgrade("Piercing", "Increases the chance of your bullets passing through enemies by 25%", 1, (p) => { p.Upgrade_Piercing(0.25f); }));
        Upgrades.Add(new Upgrade("Homing Bullets", "Your bullets will curve towards enemies", 1, (p) => { p.Upgrade_Homing(); }));
        Upgrades.Add(new Upgrade("Precise Shooting", "Increases your chance to crit for double damage by 10%", 1, (p) => { p.Upgrade_CritChance(0.1f); }));
        Upgrades.Add(new Upgrade("Faster Bullets", "Bullets travel faster", 1, (p) => { p.Upgrade_BulletSpeed(0.25f);  }));

        Upgrades.Add(new Upgrade("Scrap Magnet", "Collect scrap at longer range", 2, (p) => { p.Upgrade_ScrapRange(); }));
        Upgrades.Add(new Upgrade("Scavenger", "Increases the amount of scrap you get from enemies", 1, (p) => { WorldManager.Instance.BonusScrapChance += 0.10f; }));

        Upgrades.Add(new Upgrade("Improved Engines", "Carry +5 Scrap without slowing down", 2, (p) => { p.Upgrade_Engines(5); }));
        Upgrades.Add(new Upgrade("Phase Cloak", "Increases invulnerability timer after being hit", 3, (p) => { p.Upgrade_Invulnerability();  }));

        Upgrades.Add(new Upgrade("Improved Heat Sinks", "Weapons generate less heat", 1, (p) => { p.Improved_HeatSinks(); }));

        Upgrades.Add(new Upgrade("Mega Bomb Capacity", "Does damage to the entire screen (deploy with 'B')", 1, (p) => { p.Upgrade_Megabombs(); }));
        Upgrades.Add(new Upgrade("Mega Bomb Damage", "Increase Mega Bomb Damage", 5, (p) => { p.Upgrade_MegabombDamage(); }));

        Upgrades.Add(new Upgrade("Bullet Time", "Warp reality to slow down all enemies (deploy with 'T')", 5, (p) => { p.Upgrade_Bullettime(); }));
        //Upgrades.Add(new Upgrade("Radar", "Track off-screen enemies", 1, (p) => { /*One time*/ }));

        Upgrades.Add(new Upgrade("Auto-Drone", "A robotic companion that fires at enemies automatically", 2, (p) => { p.Upgrade_AddDrone(); }));
        //Upgrades.Add(new Upgrade("Defense Drone", "A robotic companion that shoots down incoming bullets", 1, (p) => { /* Reset Cost */ }));

        Upgrades.Add(new Upgrade("Drone Armor", "Auto-Drones will take more hits before being destroyed", 2, (p) => { p.Upgrade_DroneArmor(); }));
        Upgrades.Add(new Upgrade("Drone Fire Rate", "", 2, (p) => { p.Upgrade_DroneFireRate(); }));
        Upgrades.Add(new Upgrade("Drone Damage", "", 2, (p) => { p.Upgrade_DroneDamage(); }));
    }

    public List<Upgrade> Upgrades;

    public void PurchaseUpgrade(Upgrade upgrade)
    {
        PlayerUnit pu = GameObject.FindObjectOfType<PlayerUnit>();
        Unit unit = pu.GetComponent<Unit>();
        if (unit.Health < upgrade.Cost)
            return;

        unit.Health -= upgrade.Cost;
        upgrade.Cost = Mathf.CeilToInt(upgrade.Cost * 1.5f);
        upgrade.UpgradeAction( pu );
    }
}

public delegate void UpgradeAction(PlayerUnit p);

public class Upgrade
{
    public Upgrade(string n, string d, int c, UpgradeAction ua)
    {
        Name = n;
        Description = d;
        Cost = c;
        UpgradeAction = ua;
    }

    public string Name;
    public string Description;
    public int Cost;
    public UpgradeAction UpgradeAction;
}
