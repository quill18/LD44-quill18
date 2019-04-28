using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stats02 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        string s = "level: " + (WorldManager.Instance.GetCurrentLevel()+1);
        /*if(PlayerUnit.OverEncumbered())
        {
            s += " !!!OVERENCUMBERED !!!";
        }*/

        text.text = s;
    }
}
