using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private void OnEnable()
    {
        myPlayerUnit = GetComponentInParent<PlayerUnit>();
        weaponSlots = GetComponentsInChildren<WeaponSlot>();

        BoundsGracePeriod = 1f;

        //ScrapRolls = Health / 2 + 1;

        isDead = false;

        otherPlayerUnit = GameObject.FindObjectOfType<PlayerUnit>();

        Bounds unitBounds = new Bounds();
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            unitBounds.Encapsulate(sr.sprite.bounds);
        }


        boundsMax = unitBounds.max;
        boundsMin = unitBounds.min;


    }

    Vector3 boundsMax;
    Vector3 boundsMin;



    public int Health = 0;
    public int Damage = 1; // Damage we inflict to another unit on Collision
    public int DamageBonus = 0;
    public float PierceChance = 0;  // Chance to not take damage on collision..........
    public float HomingTurnRate = 0;
    public bool HomingRateDecays = true;
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

    public enum ExplosionType { TINY, NORMAL, BIG };
    public ExplosionType ExplosionStyle = ExplosionType.NORMAL;

    bool isDead = false;

    PlayerUnit myPlayerUnit;    // So I know if I am the player
    PlayerUnit otherPlayerUnit;

    bool IsPlayer()
    {
        return myPlayerUnit != null;
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

            if(HomingRateDecays)
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

        if (invulnerabilityLeft > 0)
        {
            invulnerabilityLeft -= BulletDeltaTime();

            //Debug.Log("Invuln: " + invulnerabilityLeft);

            SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = invulnerabilityLeft > 0 ? 0.5f : 1.0f;
                sr.color = c;
            }
        }
    }

    int homingSkipFrame = 0;
    void HomeToNearestEnemy()
    {
        // An enemy is someone on a different layer (Player or Enemy)
        homingSkipFrame--;
        if (homingSkipFrame > 0)
            return;

        homingSkipFrame = Random.Range(2,4);

        if (gameObject.layer == 12)
        {
            // I am on the enemy's team, so I need to home to the player
            if (otherPlayerUnit == null)
                return;

            HomeTo( otherPlayerUnit.transform );
            return;
        }

        if (EnemyUnit.AllEnemies == null)
            return;

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

        // Is the TOP of our sprite above the top of our screen?
        // Use ScreenBoundLimits as a border/padding
        if (boundsMax.y + this.transform.position.y > WorldManager.Instance.ScreenBounds.max.y - ScreenBoundLimits)
        {
            float overage = boundsMax.y + this.transform.position.y - WorldManager.Instance.ScreenBounds.max.y + ScreenBoundLimits;
            GetComponent<Rigidbody2D>().position += new Vector2(0, -overage);
        }
        else if (boundsMin.y + this.transform.position.y < WorldManager.Instance.ScreenBounds.min.y + ScreenBoundLimits)
        {
            float overage = boundsMin.y + this.transform.position.y - WorldManager.Instance.ScreenBounds.min.y - ScreenBoundLimits;
            GetComponent<Rigidbody2D>().position += new Vector2(0, -overage);
        }

        if (boundsMax.x + this.transform.position.x > WorldManager.Instance.ScreenBounds.max.x - ScreenBoundLimits)
        {
            float overage = boundsMax.x + this.transform.position.x - WorldManager.Instance.ScreenBounds.max.x + ScreenBoundLimits;
            GetComponent<Rigidbody2D>().position += new Vector2(-overage, 0);
        }
        else if (boundsMin.x + this.transform.position.x < WorldManager.Instance.ScreenBounds.min.x + ScreenBoundLimits)
        {
            float overage = boundsMin.x + this.transform.position.x - WorldManager.Instance.ScreenBounds.min.x - ScreenBoundLimits;
            GetComponent<Rigidbody2D>().position += new Vector2(-overage, 0);
        }

    }

    void DestroyOutOfScreenBounds()
    {

        // Is the TOP of our sprite above the top of our screen?
        if (boundsMax.y + this.transform.position.y > WorldManager.Instance.ScreenBounds.max.y + ScreenBoundLimits)
        {
            Die(true);
        }
        else if (boundsMin.y + this.transform.position.y < WorldManager.Instance.ScreenBounds.min.y - ScreenBoundLimits)
        {
            Die(true);
        }
        else if (boundsMax.x + this.transform.position.x > WorldManager.Instance.ScreenBounds.max.x + ScreenBoundLimits)
        {
            Die(true);
        }
        else if (boundsMin.x + this.transform.position.x < WorldManager.Instance.ScreenBounds.min.x - ScreenBoundLimits)
        {
            Die(true);
        }

    }


    public void TakeDamage(int amount)
    {
        if (isDead)
            Debug.Log("Dead thing taking damage.");

        if (invulnerabilityLeft > 0 || amount <= 0 || isDead)
            return;

        if( GetComponentsInChildren<Unit>().Length > 1)
        {
            // We have sub-parts, so we can't be damaged.
            return;
        }

        if (myPlayerUnit != null)
            DamageFlasher.Instance.DoFlash();

        Health -= amount;
        invulnerabilityLeft = InvulnerabilityDuration;

        if (Health < 0)  // Only die when health is NEGATIVE
        {
            Die();
        }
        else
        {
            // took damage but didn't die
            SoundManager.Instance.PlayClang();
        }
    }

    public void GainHealth(int amount)
    {
        Health += amount;
    }

    public void Die( bool justDestroy = false )
    {
        if (isDead)
            return; 

        isDead = true;

        if (justDestroy == false)
        {
            // drop loot, change score, etc....

            for (int i = 0; i < ScrapRolls; i++)
            {
                WorldManager.Instance.RollScrap(this.transform.position);
            }

            if(GetComponent<EnemyUnit>() != null)
                WorldManager.Instance.EnemiesKilled++;


            switch (ExplosionStyle)
            {
                case ExplosionType.TINY:
                    //SoundManager.Instance.PlayExplosion();
                    WorldManager.Instance.ExplosionSmall(this.transform.position);
                    break;
                case ExplosionType.NORMAL:
                    SoundManager.Instance.PlayExplosion();
                    WorldManager.Instance.Explosion(this.transform.position);
                    break;
                case ExplosionType.BIG:
                    SoundManager.Instance.PlayBigExplosion();
                    WorldManager.Instance.ExplosionBig(this.transform.position);
                    break;
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
