using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotator : MonoBehaviour
{

	[SerializeField] private Vector2 rotationRange = new Vector3(360, 360);
	[SerializeField] private float rotationSpeed = 10;
	[SerializeField] private float scalar = 0.5f;
	[SerializeField] private float dampingTime = 0.2f; // controls rot smoothness
	private Vector3 targetAngles;
	private Vector3 followAngles;
	private Vector3 followVelocity;
	private Quaternion originalRotation;

	void Start()
	{
		originalRotation = transform.localRotation;
	}

	void Update()
	{
		// we make initial calculations from the original local rotation
		transform.localRotation = originalRotation;

		float inputH = Input.GetAxis("Mouse X");
		float inputV = Input.GetAxis("Mouse Y");

		targetAngles.y += inputH * rotationSpeed;
		targetAngles.x += inputV * rotationSpeed;

		// clamp values to allowed range
		targetAngles.y = Mathf.Clamp(targetAngles.y, -rotationRange.y * scalar, rotationRange.y * scalar);
		targetAngles.x = Mathf.Clamp(targetAngles.x, -rotationRange.x * scalar, rotationRange.x * scalar);

		// smoothly interpolate current values to target angles
		followAngles = Vector3.SmoothDamp(followAngles, targetAngles, ref followVelocity, dampingTime, Mathf.Infinity, Time.deltaTime);

		// update the actual gameobject's rotation
		transform.localRotation = originalRotation * Quaternion.Euler(-followAngles.x, followAngles.y, 0);
	}
}
