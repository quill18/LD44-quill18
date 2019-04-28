using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimeUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public GameObject BulletTimeIcon;
    PlayerUnit playerUnit;

    // Update is called once per frame
    void Update()
    {
        if (playerUnit == null)
        {
            playerUnit = GameObject.FindObjectOfType<PlayerUnit>();
        }

        if (playerUnit == null)
            return;

        while (playerUnit.CurrentBullettime < this.transform.childCount)
        {
            Transform t = this.transform.GetChild(0);
            t.SetParent(null); // Become Batman
            Destroy(t.gameObject);
        }
        while (playerUnit.CurrentBullettime > this.transform.childCount)
        {
            Instantiate(BulletTimeIcon, this.transform);
        }
    }
}
