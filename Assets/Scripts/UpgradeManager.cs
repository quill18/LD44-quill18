using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Upgrades = new List<Upgrade>();

        Upgrades.Add(new Upgrade("fire rate", "increase the fire rate of your weapon by 25%", 1, (p) => { p.Upgrade_FireRate( 0.25f ); }));
        Upgrades.Add(new Upgrade("multi shot", "increases the number of bullets you fire", 1, (p) => { p.Upgrade_MultiShot(); }));
        Upgrades.Add(new Upgrade("damage", "increases the base damage of your bullets", 1, (p) => { p.Upgrade_Damage(); }));
        Upgrades.Add(new Upgrade("piercing", "increases the chance of your bullets passing through enemies by 25%", 1, (p) => { p.Upgrade_Piercing(0.25f); }));
        Upgrades.Add(new Upgrade("homing bullets", "your bullets will curve towards enemies", 1, (p) => { p.Upgrade_Homing(); }));
        Upgrades.Add(new Upgrade("precise shooting", "increases your chance to crit for double damage by 10%", 1, (p) => { p.Upgrade_CritChance(0.1f); }));
        Upgrades.Add(new Upgrade("faster bullets", "bullets travel faster", 1, (p) => { p.Upgrade_BulletSpeed(0.25f);  }));

        Upgrades.Add(new Upgrade("scrap scoop", "collect scrap at longer range", 2, (p) => { p.Upgrade_ScrapRange(); }));
        Upgrades.Add(new Upgrade("scavenger", "increases the amount of scrap you get from enemies", 1, (p) => { WorldManager.Instance.BonusScrapChance += 0.01f; }));

        //Upgrades.Add(new Upgrade("Improved Engines", "Carry +5 Scrap without slowing down", 2, (p) => { p.Upgrade_Engines(5); }));
        Upgrades.Add(new Upgrade("phase cloak", "increases invulnerability timer after being hit", 2, (p) => { p.Upgrade_Invulnerability();  }));

        Upgrades.Add(new Upgrade("improved heat sinks", "weapons generate less heat", 1, (p) => { p.Improved_HeatSinks(); }));

        Upgrades.Add(new Upgrade("mega bomb capacity", "does damage to the entire screen (deploy with 'b')", 1, (p) => { p.Upgrade_Megabombs(); AddMegaBombUpgrades();  }));

        Upgrades.Add(new Upgrade("bullet time", "warp reality to slow down all enemies (deploy with 't')", 2, (p) => { p.Upgrade_Bullettime(); }));
        //Upgrades.Add(new Upgrade("Radar", "Track off-screen enemies", 1, (p) => { /*One time*/ }));

        Upgrades.Add(new Upgrade("auto-drone", "a robotic companion that fires at enemies automatically", 2, (p) => { p.Upgrade_AddDrone(); AddDroneUpgrades(); }));
        //Upgrades.Add(new Upgrade("Defense Drone", "A robotic companion that shoots down incoming bullets", 1, (p) => { /* Reset Cost */ }));

    }

    public List<Upgrade> Upgrades;

    bool didBombUpgrades = false;
    void AddMegaBombUpgrades()
    {
        if (didBombUpgrades)
            return;

        didBombUpgrades = true;
        Upgrades.Add(new Upgrade("mega bomb damage", "increase mega bomb damage", 2, (p) => { p.Upgrade_MegabombDamage(); }));

    }

    bool didDroneUpgrades = false;
    void AddDroneUpgrades()
    {
        if (didDroneUpgrades)
            return;

        didDroneUpgrades = true;
        Upgrades.Add(new Upgrade("drone armor", "auto-drones will take more hits before being destroyed", 1, (p) => { p.Upgrade_DroneArmor(); }));
        Upgrades.Add(new Upgrade("drone fire rate", "drones will fire faster", 1, (p) => { p.Upgrade_DroneFireRate(); }));
        Upgrades.Add(new Upgrade("drone damage", "drones will do more damage", 1, (p) => { p.Upgrade_DroneDamage(); }));

    }

    public void PurchaseUpgrade(Upgrade upgrade)
    {
        PlayerUnit pu = GameObject.FindObjectOfType<PlayerUnit>();
        Unit unit = pu.GetComponent<Unit>();
        if (unit.Health < upgrade.Cost)
            return;

        SoundManager.Instance.PlayUpgradePurchased();

        unit.Health -= upgrade.Cost;
        upgrade.Cost = Mathf.CeilToInt(upgrade.Cost * 2f);
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
