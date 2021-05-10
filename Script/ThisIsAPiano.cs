using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisIsAPiano : MonoBehaviour
{
    public List<AudioClip> pianoSounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPianoSound()
    {
        int randomSound = Random.Range(0, 3);

        GetComponent<AudioSource>().clip = pianoSounds[randomSound];
        GetComponent<AudioSource>().Play();
    }
}
