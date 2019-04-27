using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeatMeter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        playerUnit = GameObject.FindObjectOfType<PlayerUnit>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponentInChildren<Image>();
    }

    PlayerUnit playerUnit;
    TextMeshProUGUI text;
    Image image;

    // Update is called once per frame
    void Update()
    {
        // Average heat from all weapons (although we only have one right now)

        if (playerUnit == null)
            return;

        WeaponSlot[] wss = playerUnit.GetComponentsInChildren<WeaponSlot>();
        float heat = 0;
        foreach(WeaponSlot ws in wss)
        {
            heat += ws.Heat;
        }
        heat /= wss.Length;

        text.text = "Heat: " + Mathf.CeilToInt(heat*100);
        image.fillAmount = heat;
    }
}
