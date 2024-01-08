using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBoat : MonoBehaviour
{

    private ParticleSystem boatExplosion;

    private AudioSource explosionAudioSource;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("destinationArea"))
        {
            Debug.Log("Boat reached destination");
            StartCoroutine(StartExplosion());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        boatExplosion = transform.GetChild(0).GetComponent<ParticleSystem>();
        explosionAudioSource = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartExplosion()
    {
        explosionAudioSource.Play();
        boatExplosion.Play();
        yield return new WaitForSeconds(20.0f);
        StartCoroutine(StartExplosion());
    }

}
