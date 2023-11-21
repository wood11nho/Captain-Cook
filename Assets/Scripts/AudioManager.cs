using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public AudioSource backgroundMusic;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeBackgroundMusic(AudioClip currentMusic)
    {
        backgroundMusic.Stop();
        backgroundMusic.clip = currentMusic;
        backgroundMusic.Play();
    }

}
