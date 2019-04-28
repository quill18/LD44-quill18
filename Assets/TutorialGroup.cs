using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGroup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }

            gameObject.SetActive(false);
            WorldManager.Instance.UnPause();
        }

    }
}
