using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioSource audioSource1; // Game Intro Background Music
    public AudioSource audioSource2; // Normal State Background Music
    // Start is called before the first frame update
    void Start()
    {
        audioSource1 = GameObject.Find("Intro Background Music").GetComponent<AudioSource>();
        audioSource2 = GameObject.Find("Normal State Background Music").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource1.isPlaying && !audioSource2.isPlaying)
        {
            audioSource2.Play();
        }
    }
}
