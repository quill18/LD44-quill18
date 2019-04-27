using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stats01 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        PlayerUnit = GameObject.FindObjectOfType<PlayerUnit>();
    }

    TextMeshProUGUI text;
    PlayerUnit PlayerUnit;

    // Update is called once per frame
    void Update()
    {
        if (PlayerUnit == null)
            return;

        string s = "Scrap: " + PlayerUnit.ScrapCount();
        if(PlayerUnit.OverEncumbered())
        {
            s += " !!!OVERENCUMBERED !!!";
        }

        text.text = s;
    }
}
