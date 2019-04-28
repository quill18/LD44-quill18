using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    AudioSource source;
    public AudioClip[] Songs;
    int currSong = 0;

    // Update is called once per frame
    void Update()
    {
        if(source.isPlaying == false)
        {
            source.clip = Songs[currSong];
            source.Play();
            currSong = (currSong + 1) % Songs.Length;
        }
    }
}
