using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSound : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        source = GetComponent<AudioSource>();
        oneFrame = false;
    }

    AudioSource source;
    bool oneFrame = false;

    // Update is called once per frame
    void Update()
    {
        if(oneFrame == false)
        {
            if (source.isPlaying)
                oneFrame = true;
            else
                source.Play();

            return;
        }

        if (source.isPlaying == false)
            SimplePool.Despawn(gameObject);
    }
}
