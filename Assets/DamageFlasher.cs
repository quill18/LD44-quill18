using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlasher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        image = GetComponent<Image>();
    }

    Image image;
    static public DamageFlasher Instance;

    float flashTime = 0;
    float alphaOn = 0.11f;

    float nextCycle = 0;
    bool flashIsOn;

    // Update is called once per frame
    void Update()
    {
        if (flashTime > 0)
        {
            flashTime -= Time.deltaTime;
            if(flashTime <= 0)
            {
                FlashOff();
                return;
            }

            nextCycle -= Time.deltaTime;

            if(nextCycle <= 0)
            {
                if (flashIsOn)
                    FlashOff();
                else
                    FlashOn();
            }

        }
    }

    void FlashOn()
    {
        flashIsOn = true;
        nextCycle = 0.05f;
        Color c = image.color;
        c.a = alphaOn;
        image.color = c;
    }

    void FlashOff()
    {
        flashIsOn = false;
        nextCycle = 0.05f;
        Color c = image.color;
        c.a = 0;
        image.color = c;

    }

    public void DoFlash()
    {
        flashTime = 0.25f;
        FlashOn();
    }
}
