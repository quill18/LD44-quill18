using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        _LifeSpan = LifeSpan;

        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            Color c = sr.color;
            c.a = 1;
            sr.color = c;

        }
    }

    public float LifeSpan = 1f;
    float _LifeSpan;
    public float FadeOutTime = 0;

        // Update is called once per frame
    void Update()
    {
        if (WorldManager.Instance.IsPaused)
            return;

        _LifeSpan -= WorldManager.Instance.BulletDeltaTime();
        if(_LifeSpan <= 0)
        {
            SimplePool.Despawn(gameObject);
        }
        else if(_LifeSpan <= FadeOutTime)
        {
            SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = Mathf.Lerp(0f, 1f, _LifeSpan / FadeOutTime);
                sr.color = c;
            }
        }
    }
}
