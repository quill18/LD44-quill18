using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotEnabler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check to see if there are any unit sub-modules on this enemy
        // If no, activate the guns!

        WeaponSlot[] wss = GetComponentsInChildren<WeaponSlot>(false);

        if (wss == null || wss.Length == 0)
        {
            GetComponent<WeaponSlot>().enabled = true;
            this.enabled = false;
        }

            
    }
}
