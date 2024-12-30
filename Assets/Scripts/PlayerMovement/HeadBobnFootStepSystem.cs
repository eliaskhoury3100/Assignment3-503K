using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class HeadBobnFootStepSystem : MonoBehaviour
{

	[Range(0.001f, 0.01f)]
	[SerializeField] private float Amount = 0.002f;

	[Range(1f, 30f)]
	[SerializeField] private float Frequency = 10.0f;

	[Range(10f, 100f)]
	[SerializeField] private float Smooth = 10.0f;

	private Vector3 StartPos;

	// for footstep mechanism synchro with headbob
	[Range(0, 20f)]
	[SerializeField] private float frequency = 10.0f;

	// Add an array for the footstep sounds
	[SerializeField] private AudioClip[] footstepSounds;

	// Add an AudioClip for the heavy breathing sound
	[SerializeField] private AudioClip breathing;
	private int breathCount = 0;
	[SerializeField] private int stepsBeforeBreathing = 10;

	// Reference to the AudioSource
	private AudioSource audioSource;

	float Sin;

	bool isTriggered = false;


	void Start()
	{
		StartPos = transform.localPosition;
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{

		CheckForHeadbobnFootStepTrigger();
		StopHeadbob();

	}


	private void CheckForHeadbobnFootStepTrigger()
	{
		float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

		if (inputMagnitude > 0)
		{
			StartHeadBobnFootStep();

		}
	}

	private Vector3 StartHeadBobnFootStep()
	{
		Vector3 pos = Vector3.zero;
		pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
		pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime); // at half freq of y
		transform.localPosition += pos;

		// for footsteps
		Sin = Mathf.Sin(Time.time * frequency);

		if (Sin > 0.97f && isTriggered == false)
		{

			isTriggered = true;
			//Debug.Log("Tic");
			//audioSource.PlayOneShot(breathing);
			// Only play the breathing sound every few steps
			if (breathCount >= stepsBeforeBreathing)
			{
				audioSource.PlayOneShot(breathing); // Adjust the volume
				breathCount = 0; // Reset the step counter
			}
			breathCount++;
			PlayFootstepSound();

		}
		else if (isTriggered == true && Sin < -0.97f)
		{

			isTriggered = false;
		}

		return pos;
	}

	// Method to play a random footstep sound
	private void PlayFootstepSound()
	{
		// Pick a random sound from the footstepSounds array
		int randomIndex = Random.Range(0, footstepSounds.Length);
		AudioClip clip = footstepSounds[randomIndex];

		// Play the selected sound
		audioSource.PlayOneShot(clip);
	}

	// guarantee return to same initial pos
	private void StopHeadbob()
	{
		if (transform.localPosition == StartPos) return;
		transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, 1 * Time.deltaTime);
	}
}
