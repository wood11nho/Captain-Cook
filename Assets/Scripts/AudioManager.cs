using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public AudioSource backgroundMusic;

    public AudioClip winMusic;

    public AudioClip loseMusic;

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

    public void changeToWinMusic()
    {
        backgroundMusic.Stop();
        backgroundMusic.clip = winMusic;
        backgroundMusic.Play();
    }

    public void changeToLoseMusic()
    {
        backgroundMusic.Stop();
        backgroundMusic.clip = loseMusic;
        backgroundMusic.Play();
    }

}
