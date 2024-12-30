using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCollectibles : MonoBehaviour
{
    [SerializeField] private float rotSpeed = 100f;  // Rotation speed of the collectibles
    [SerializeField] private AudioClip collectionSound;

    void Update()
    {
        // Rotate collectible around its own Y-axis
        this.transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Create a new GameObject to hold the AudioSource
            GameObject audioHolder = new GameObject("AudioHolder");
            AudioSource tempAudioSource = audioHolder.AddComponent<AudioSource>(); // add AudioSource component
            tempAudioSource.clip = collectionSound; // assign sound effect 
            tempAudioSource.Play();

            // Destroy the temporary GameObject after the clip finishes playing
            Destroy(audioHolder, collectionSound.length);

            // Increase score by 1
            ManagerCollectibles.AddScore(1);

            // Destroy the collectible instantly
            Destroy(gameObject);
        }
    }

}