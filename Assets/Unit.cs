using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        playerUnit = GetComponentInParent<PlayerUnit>();
        weaponSlots = GetComponentsInChildren<WeaponSlot>();

    }

    private void OnEnable()
    {
        BoundsGracePeriod = 5;

        ScrapRolls = Health / 2 + 2;

    }

    public int Health = 0;
    public int Damage = 1; // Damage we inflict to another unit on Collision
    public int DamageBonus = 0;
    public float PierceChance = 0;  // Chance to not take damage on collision..........
    public float HomingTurnRate = 0;
    public float CritChance = 0;

    public float InvulnerabilityDuration = 0;
    float invulnerabilityLeft = 0;

    public Vector3 Velocity;
    public float MaxSpeed = 5;

    public bool LimitedToScreenBounds = false;
    public float ScreenBoundLimits = 10;
    float BoundsGracePeriod;

    WeaponSlot[] weaponSlots;


    public bool FaceVelocity; // Look in direction we're moving

    public int ScrapRolls = 1; // How many times we roll to see if we drop scrap

    PlayerUnit playerUnit;
    bool IsPlayer()
    {
        return playerUnit != null;
    }

    float BulletDeltaTime()
    {
        if (IsPlayer())
            return Time.deltaTime;

        return WorldManager.Instance.BulletDeltaTime();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (WorldManager.Instance.IsPaused)
            return;

        UpdateInvulnerability();


        if (HomingTurnRate > 0)
        {
            // This enemy or bullet wants to home into a target
            HomeToNearestEnemy();

            HomingTurnRate *= 1f - (0.50f * WorldManager.Instance.BulletDeltaTime());
        }

        GetComponent<Rigidbody2D>().position += (Vector2)Velocity * BulletDeltaTime();

        // Have we left the screen bounds?

        if(LimitedToScreenBounds)
        {
            KeepInScreenBounds();
        }
        else
        {
            BoundsGracePeriod -= Time.deltaTime;
            if (BoundsGracePeriod <= 0)
            {
                DestroyOutOfScreenBounds();
            }
        }


    }

    void UpdateInvulnerability()
    {
        invulnerabilityLeft -= BulletDeltaTime();

        if( InvulnerabilityDuration > 0)
        {
            SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = invulnerabilityLeft > 0 ? 0.5f : 1.0f;
            }
        }
    }

    void HomeToNearestEnemy()
    {
        // An enemy is someone on a different layer (Player or Enemy)

        if(gameObject.layer == 12)
        {
            // I am on the enemy's team, so I need to home to the player
            PlayerUnit pu = GameObject.FindObjectOfType<PlayerUnit>();
            if (pu == null)
                return;

            HomeTo( pu.transform );
            return;
        }

        EnemyUnit nearest = null;
        float dist = Mathf.Infinity;

        foreach (EnemyUnit e in EnemyUnit.AllEnemies)
        {
            float d = Vector3.Distance(e.transform.position, this.transform.position);
            if (nearest == null || d < dist)
            {
                nearest = e;
                dist = d;
            }
        }

        if(nearest != null)
        {
            HomeTo(nearest.transform);
        }
    }

    void HomeTo(Transform t)
    {
        // Turn towards the target

        Vector3 dirToTarget = t.position - this.transform.position;
        float strength = 1;// Mathf.Clamp01( 8 / dirToTarget.sqrMagnitude );

        float angle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, HomingTurnRate * strength * BulletDeltaTime());
        Velocity = transform.right * Velocity.magnitude;
    }

    void KeepInScreenBounds()
    {
        Bounds unitBounds = new Bounds();
        Collider2D[] cols = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in cols)
        {
            unitBounds.Encapsulate(col.bounds);
        }


        // Is the TOP of our sprite above the top of our screen?
        // Use ScreenBoundLimits as a border/padding
        if (unitBounds.max.y > WorldManager.Instance.ScreenBounds.max.y - ScreenBoundLimits)
        {
            float overage = unitBounds.max.y - WorldManager.Instance.ScreenBounds.max.y + ScreenBoundLimits;
            GetComponent<Rigidbody2D>().position += new Vector2(0, -overage);
        }
        else if (unitBounds.min.y < WorldManager.Instance.ScreenBounds.min.y + ScreenBoundLimits)
        {
            float overage = unitBounds.min.y - WorldManager.Instance.ScreenBounds.min.y - ScreenBoundLimits;
            GetComponent<Rigidbody2D>().position += new Vector2(0, -overage);
        }

        if (unitBounds.max.x > WorldManager.Instance.ScreenBounds.max.x - ScreenBoundLimits)
        {
            float overage = unitBounds.max.x - WorldManager.Instance.ScreenBounds.max.x + ScreenBoundLimits;
            GetComponent<Rigidbody2D>().position += new Vector2(-overage, 0);
        }
        else if (unitBounds.min.x < WorldManager.Instance.ScreenBounds.min.x + ScreenBoundLimits)
        {
            float overage = unitBounds.min.x - WorldManager.Instance.ScreenBounds.min.x - ScreenBoundLimits;
            GetComponent<Rigidbody2D>().position += new Vector2(-overage, 0);
        }

    }

    void DestroyOutOfScreenBounds()
    {
        Bounds unitBounds = new Bounds();
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            unitBounds.Encapsulate(sr.bounds);
        }

        // Is the TOP of our sprite above the top of our screen?
        if (unitBounds.max.y > WorldManager.Instance.ScreenBounds.max.y + ScreenBoundLimits)
        {
            Die(true);
        }
        else if (unitBounds.min.y < WorldManager.Instance.ScreenBounds.min.y - ScreenBoundLimits)
        {
            Die(true);
        }
        else if (unitBounds.max.x > WorldManager.Instance.ScreenBounds.max.x + ScreenBoundLimits)
        {
            Die(true);
        }
        else if (unitBounds.min.x < WorldManager.Instance.ScreenBounds.min.x - ScreenBoundLimits)
        {
            Die(true);
        }

    }


    public void TakeDamage(int amount)
    {
        if (invulnerabilityLeft > 0)
            return;

        Health -= amount;
        invulnerabilityLeft = InvulnerabilityDuration;

        if (Health < 0)  // Only die when health is NEGATIVE
        {
            Die();
        }
    }

    public void GainHealth(int amount)
    {
        Health += amount;
    }

    public void Die( bool justDestroy = false )
    {

        if(justDestroy == false)
        {
            // drop loot, change score, etc....

            for (int i = 0; i < ScrapRolls; i++)
            {
                WorldManager.Instance.RollScrap(this.transform.position);
            }
        }

        SimplePool.Despawn(gameObject); // Reminder: Destroy only truly resolves between frames
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // We are in collision with a Unit from another faction, we will inflict our Damage on it.
        Unit other = collision.GetComponentInParent<Unit>();
        if(other == null)
        {
            // We collided against something that isn't a Unit, so we can't damage it.
            return;
        }

        // If the other object has a "pierce chance", it's possible that we won't do damage to it.
        float r = Random.Range(0f, 1f);
        if(r < other.PierceChance)
        {
            other.PierceChance -= 1f;
        }
        else
        {
            int dmg = Damage + DamageBonus;

            float c = CritChance;
            while (c > 0)
            {
                if( Random.Range(0f, 1f) < c )
                {
                    dmg *= 2;
                }
                c -= 1;
            }

            other.TakeDamage(dmg);
        }

    }

    public void Set_FireRateBonus(float amt)
    {
        foreach (WeaponSlot ws in weaponSlots)
        {
            ws.FireRateBonus = amt;
        }

    }


}
