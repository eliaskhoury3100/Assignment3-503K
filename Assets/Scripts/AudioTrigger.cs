using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFXOnTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;  // Assign your audio source in the Inspector
    [SerializeField] private float initialVolume;
    private float fadeDuration = 1.0f;  // Duration of the fade-out in seconds

    private void Start()
    {
        audioSource.volume = initialVolume;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Ensure the player has the "Player" tag
        {
            audioSource.Play();
            StopAllCoroutines();  // Stop fade-out if player re-enters before it ends
            audioSource.volume = initialVolume;  // Reset volume to initial when re-entering
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeOutAudio());
        }
    }

    IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;  // Wait for the next frame
        }

        audioSource.Stop();  // Stop the audio after fading out
        audioSource.volume = startVolume;  // Reset the volume for future use
    }
}