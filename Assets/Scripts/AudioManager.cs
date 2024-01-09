using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public AudioSource backgroundMusic;

    public AudioClip winMusic;

    public AudioClip loseMusic;

    [SerializeField]
    AudioSource birdsSound;

    void Start()
    {
        StartCoroutine(PlayBirdsSoundRandomly());
    }

    IEnumerator PlayBirdsSoundRandomly()
    {
        while (true)
        {
            float waitTime = Random.Range(20f, 40f);
            yield return new WaitForSeconds(waitTime);

            if (!birdsSound.isPlaying)
            {
                birdsSound.Play();
            }
        }
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
    public void stopBackgroundMusic()
    {
        backgroundMusic.Stop();
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
