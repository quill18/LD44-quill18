using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        CooldownLeft = Random.Range(InitialFireDelay / 2f, InitialFireDelay * 1.5f);
    }

    public float InitialFireDelay = 1f;
    public float FireDelay = 0.5f;
    public float FireRateBonus = 0;
    public float LongDelay = 1f;
    public float LongDelayCount = 0;
    int count = 0;
    float CooldownLeft = 0;
    public GameObject BulletPrefab;
    public Vector3 offset;
    public int multishot = 0;
    public int DamageBonus = 0;
    public float PierceChance = 0;
    public float HomingTurnRate;
    public float CritChance;
    public float SpeedBonus;

    public float Heat;
    public float HeatPerShot = 0;
    public float HeatSink;
    public bool Overheated;

    public ParticleSystem HeatEffects;

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + offset, .1f);
        playerUnit = GetComponentInParent<PlayerUnit>();
    }


    // Update is called once per frame
    void Update()
    {
        CooldownLeft -= BulletDeltaTime() * (1f + FireRateBonus);
        Heat = Mathf.Max(Heat - HeatSink * BulletDeltaTime(), 0);
        if (Heat <= 0)
            Overheated = false;

        if (HeatEffects != null)
        {
            if ( Heat > 0.5f || Overheated)
            {
                if(HeatEffects.isStopped)
                    HeatEffects.Play();
            }
            else 
            {
                if (HeatEffects.isPlaying)
                    HeatEffects.Stop();
            }
        }
    }

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


    bool evenShot = false;

    public void Fire()
    {
        if (!CanFire())
            return;

        SoundManager.Instance.PlayPew();

        Heat += HeatPerShot * (Mathf.Sqrt(multishot*2f + 1));

        count++;

        if (LongDelayCount > 0 && count >= LongDelayCount)
        {
            CooldownLeft = LongDelay;
            count = 0;
        }
        else
        {
            CooldownLeft = FireDelay;
        }

        if (Heat >= 1f)
        {
            Overheated = true;
        }


        if (multishot == 0)
        {
            SpawnBullet(0);
            return;             // EARLY EXIT IF NO MULTISHOT
        }

        float a = 0;
        float b = 1;
        for (int i = 0; i < multishot; i++)
        {
            a += b;
            b /= 2;
        }
        //  b = ((1.0f - 0.5f **a)* 2.0f)


        float totalAngle = (45f / 2f) * a;

        float angleSpread = (totalAngle / (float)multishot);

        if (evenShot == false && (multishot % 2) == 1)
        {
            evenShot = true;
        }
        else
        {
            evenShot = false;
        }



        for (int i = -multishot/2; i <= (multishot+1)/2; i++)
        {
            float angle = angleSpread * (float)i;
            if (evenShot)
                angle -= angleSpread;

            SpawnBullet( angle );
        }


    }

    public bool FacesLeft = true;

    void SpawnBullet(float angle)
    {
        // Need to add in the Unit's actual facing to this offset
        if (FacesLeft)
            angle += 180;

        GameObject go = SimplePool.Spawn(BulletPrefab, this.transform.position + offset, Quaternion.identity);
        go.layer = this.gameObject.layer+3;
        foreach (Collider2D c in go.GetComponentsInChildren<Collider2D>())
        {
            c.gameObject.layer = this.gameObject.layer+3;
        }

        Unit u = go.GetComponent<Unit>();
        u.DamageBonus = DamageBonus;
        u.PierceChance = PierceChance;
        u.HomingTurnRate = HomingTurnRate;
        u.CritChance = CritChance;
        u.Velocity = this.transform.rotation * Quaternion.Euler(0, 0, angle) * new Vector3(u.MaxSpeed * (1f + SpeedBonus), 0, 0);

        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        u.transform.rotation = q;

    }

    public bool CanFire()
    {
        return !Overheated && CooldownLeft <= 0;
    }
}
