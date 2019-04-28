using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
    static public SoundManager Instance;

    public GameObject AudioClipPlayerPrefab;

    public AudioClip[] Explosions;
    public AudioClip[] BigExplosions;

    public AudioClip[] Pews;
    public AudioClip[] Slurp;
    public AudioClip[] Clangs;

    public AudioClip[] OpenShop;
    public AudioClip[] UpgradePurchased;

    AudioSource NewSource()
    {
        GameObject go = SimplePool.Spawn(AudioClipPlayerPrefab, Vector3.zero, Quaternion.identity);
        go.transform.parent = this.transform;
        AudioSource src = go.GetComponent<AudioSource>();
        return src;
    }


    public void PlayOpenShop()
    {
        NewSource().clip = OpenShop[Random.Range(0, OpenShop.Length)];
    }

    public void PlayUpgradePurchased()
    {
        NewSource().clip = UpgradePurchased[Random.Range(0, UpgradePurchased.Length)];
    }

    public void PlayClang()
    {
        NewSource().clip = Clangs[Random.Range(0, Clangs.Length)];
    }

    public void PlayExplosion()
    {
        NewSource().clip = Explosions[Random.Range(0, Explosions.Length)];
    }

    public void PlayBigExplosion()
    {
        NewSource().clip = BigExplosions[Random.Range(0, BigExplosions.Length)];
    }

    public void PlayPew()
    {
        NewSource().clip = Pews[Random.Range(0, Pews.Length)];
    }

    public void PlaySlurp()
    {
        NewSource().clip = Slurp[Random.Range(0, Slurp.Length)];
    }
}
