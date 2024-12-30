using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween library, a popular tweening engine used in Unity to animate values over time, providing smooth transitions.

public class FpsMovement : MonoBehaviour
{

	//walking
	[Space(20)] // adds spacing in Inspector
	[SerializeField] private bool enableWalk = true;
	private Rigidbody rb;
	[SerializeField] private float walkSpeed = 5f;
	private Vector3 playerInput;
	private Vector3 velocity;
	private Vector3 velocityChange;

	//jumping
	[Space(20)]
	[SerializeField] private bool enableJump = true;
	[SerializeField] private float jumpPower = 5f;
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;
	private bool isJumping;
	private bool isGrounded;

	//sprinting
	[Space(20)]
	[SerializeField] private bool enableSprint = true;
	[SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
	[SerializeField] private float sprintSpeed = 7f;
	private float originalWalkSpeed;
	private bool isSprinting = false;

	//Crouching
	[Space(20)]
	[SerializeField] private bool enableCrouch = true;
	[SerializeField] private bool holdToCrouch = true;
	[SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
	[SerializeField] private float crouchHeight = .75f;
	[SerializeField] private float speedReduction = .5f;
	[SerializeField] private CapsuleCollider capsuleColl;
	[SerializeField] private Transform cameraPivot;
	private bool isCrouched = false;
	private Vector3 originalCapsuleCenter;
	private Vector3 jointOriginalPos;
	private float capsuleHeight;

	private void Start()
	{

		rb = GetComponent<Rigidbody>();
		capsuleColl = GetComponent<CapsuleCollider>();
		originalCapsuleCenter = capsuleColl.center;
		capsuleHeight = capsuleColl.height;
		originalWalkSpeed = walkSpeed;

	}

	private void Update()
	{

		// jumping mechanism: if enabled, key pressed, and grounded
		if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
		{
			Jump();
		}

		// sprinting mechanism: press and release key
		if (enableSprint)
		{
			if (Input.GetKeyDown(sprintKey))
			{
				isSprinting = true;
				Sprint();
			}
			else if (Input.GetKeyUp(sprintKey))
			{
				isSprinting = false;
				Sprint();
			}
		}

		// crouching mechanism
		if (enableCrouch)
		{
			// pressed crouch once
			if (Input.GetKeyDown(crouchKey) && !holdToCrouch)
			{
				Crouch();
			}
			// holding crouch
			if (Input.GetKeyDown(crouchKey) && holdToCrouch)
			{
				isCrouched = true;
				Crouch();
			}
			// releasing crouch
			else if (Input.GetKeyUp(crouchKey) && holdToCrouch)
			{
				isCrouched = false;
				Crouch();
			}
		}

		// update isGrounded bool
		CheckGround();

	}

	void FixedUpdate()
	{
	// update for walking !
		if (enableWalk)

			playerInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		playerInput = transform.TransformDirection(playerInput) * walkSpeed;

		velocity = rb.velocity;
		velocityChange = (playerInput - velocity);

		velocityChange.y = 0;

		rb.AddForce(velocityChange, ForceMode.VelocityChange);
	}

	// more accurate than colliders & layers 
	private void CheckGround()
	{

		Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z); // raycast begins near the player’s feet by lowering y
		Vector3 direction = transform.TransformDirection(Vector3.down);
		float distance = .75f;

		// ray was casted
		if (Physics.Raycast(origin, direction, out RaycastHit hit, distance)) // 'out RaycastHit hit' stores information about what the ray hits(e.g., the ground).It is used here to detect whether the ray intersects with anything.
		{
			Debug.DrawRay(origin, direction * distance, Color.red); // draw a red line in the scene view to represent the ray being cast
			isGrounded = true;
			isJumping = false;
		}
		else
		{
			isGrounded = false;
			isJumping = true;
		}
	}

	// Jump method
	private void Jump()
	{
		if (isGrounded)
		{
			rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
			isGrounded = false;
		}
	}

	private void Sprint()
	{
		if (!isSprinting)
		{
			// revert back to walking speed
			DOTween.To(() => walkSpeed,
				x => walkSpeed = x, originalWalkSpeed,
				2);

			isSprinting = true; // ensure state change
		}
		else
		{
			// increase to sprinting speed
			DOTween.To(() => walkSpeed,
				x => walkSpeed = x, sprintSpeed,
				2);

			isSprinting = false;
		}
	}

	private void Crouch()
	{
		if (!isCrouched)
		{
			// revert back to uncrouch collider center pos
			DOTween.To(() => capsuleColl.center,
				x => capsuleColl.center = x,
				originalCapsuleCenter, 2);
			// revert back to uncrouch collider height pos
			DOTween.To(() => capsuleColl.height,
				x => capsuleColl.height = x, 2,
				2);
			// revert back to uncrouch camera y-pos
			DOTween.To(() => cameraPivot.transform.localPosition,
				x => cameraPivot.transform.localPosition = x,
				new Vector3(0, 0.5f, 0), 2);
			// revert back to normal walking speed
			DOTween.To(() => walkSpeed,
				x => walkSpeed = x, originalWalkSpeed,
				2);

			isCrouched = true;
		}
		else
		{
			DOTween.To(() => capsuleColl.center,
				x => capsuleColl.center = x,
				new Vector3(0, -0.5f, 0), 2);

			DOTween.To(() => capsuleColl.height,
				x => capsuleColl.height = x, 1,
				2);

			DOTween.To(() => cameraPivot.transform.localPosition,
				x => cameraPivot.transform.localPosition = x,
				new Vector3(0, 0.0f, 0), 2);

			DOTween.To(() => walkSpeed,
				x => walkSpeed = x, speedReduction,
				2);

			isCrouched = false;
		}
	}
}
