using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapShield : MonoBehaviour
{
    // Monitors unit health and ensures that the scrap amount matches


    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();

        shieldParent = new GameObject();
        shieldParent.transform.SetParent(this.transform);
        shieldParent.transform.localPosition = Vector3.zero;
    }

    public GameObject ScrapShieldPrefab;
    public GameObject ScrapDebrisPrefab;  // Might look like debris or explode into dust or something
    Unit unit;

    GameObject shieldParent;

    float shieldRadius = 0.6f;
    float rotOffset = 0;
    float debrisSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        rotOffset += Time.deltaTime * 90;

        while (shieldParent.transform.childCount > unit.Health)
        {
            LoseScrap();
        }

        while (shieldParent.transform.childCount < unit.Health)
        {
            GainScrap();
        }

        RotateScrap();
    }

    public int ScrapCount()
    {
        return shieldParent.transform.childCount;
    }

    void LoseScrap()
    {
        // Fling off a piece of scrap
        Transform t = shieldParent.transform.GetChild( shieldParent.transform.childCount - 1 );
        t.SetParent(null); // BECOME BATMAN
        SimplePool.Despawn(t.gameObject);

        // Health changes while paused are probably from shopping, so don't do debris effect
        if (WorldManager.Instance.IsPaused)
            return;

        GameObject go = SimplePool.Spawn(ScrapDebrisPrefab, t.position, t.rotation);
        // Give it momentum in an appropriate direction
        Vector3 dir = go.transform.position - this.transform.position;
        go.GetComponent<SimpleMove>().Velocity = dir.normalized * debrisSpeed;
    }

    void GainScrap()
    {
        Debug.Log("Old GainScrap was run.");
        // Spawn a piece of scrap
        GameObject go = SimplePool.Spawn(ScrapShieldPrefab, this.transform.position, Quaternion.identity);
        go.transform.SetParent(shieldParent.transform);

    }

    public void GainScrap(Scrap scrap)
    {
        // Spawn a piece of scrap
        GameObject go = SimplePool.Spawn(ScrapShieldPrefab, scrap.transform.position, Quaternion.identity);
        go.transform.SetParent(shieldParent.transform);
        go.GetComponentInChildren<SpriteRenderer>().sprite = scrap.GetComponentInChildren<SpriteRenderer>().sprite;

    }



    void RotateScrap()
    {
        float angle = 360f / (float)shieldParent.transform.childCount;

        for (int i = 0; i < shieldParent.transform.childCount; i++)
        {
            Transform t = shieldParent.transform.GetChild(i);

            t.localPosition = Vector3.Lerp(t.localPosition,
                Quaternion.Euler(0, 0, angle * (float)i + rotOffset) * new Vector3( shieldRadius, 0, 0),
                0.1f);
        }
    }
}
