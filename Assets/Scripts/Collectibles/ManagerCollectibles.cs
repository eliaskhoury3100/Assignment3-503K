using UnityEngine;
using TMPro;

public class ManagerCollectibles : MonoBehaviour
{
    public static int score = 0;  // Static score variable accessible from anywhere in the game
    private int nbrCollectibles = 8;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject portal;  // Reference to the Portal GameObject

    private AudioSource audioSource;  
    [SerializeField] private AudioClip activationSound;  
    private bool portalActivated = false;  // Flag to ensure activation happens only once (fix distortion in sound)

    // Method to increase the score, also accessible from anywhere in the game
    public static void AddScore(int value)
    {
        score += value;
        Debug.Log("Score: " + score);  // Output the current score to the console
    }

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();  
    }

    private void Update()
    {
        // Update UI score
        scoreText.text = "Collectibles: " + score + " / " + nbrCollectibles;

        // Ensure the sound and portal activation happen only once
        if (score == nbrCollectibles && !portalActivated)
        {
            portalActivated = true;  // Set to true so the block runs only once
            audioSource.PlayOneShot(activationSound);  // Play the sound
            portal.SetActive(true);  // Activate the portal
            Debug.Log("Portal is activated!");  
        }
    }
}