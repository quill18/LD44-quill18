using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBombUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        playerUnit = GameObject.FindObjectOfType<PlayerUnit>();
    }

    public GameObject MegaBombIcon;
    PlayerUnit playerUnit;

    // Update is called once per frame
    void Update()
    {
        while (playerUnit.CurrentMegabombs < this.transform.childCount)
        {
            Transform t = this.transform.GetChild(0);
            t.SetParent(null); // Become Batman
            Destroy(t.gameObject);
        }
        while (playerUnit.CurrentMegabombs > this.transform.childCount)
        {
            Instantiate(MegaBombIcon, this.transform);
        }
    }
}
