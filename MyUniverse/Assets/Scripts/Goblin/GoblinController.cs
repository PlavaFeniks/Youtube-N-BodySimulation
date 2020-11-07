using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{

	public float stickToGroundForce; //minor force to help stick to the ground when not jumping, "Should" be replaced with surface gravity
	public Transform groundCheck; //given to empty object
	public float groundDistance; //distance to ground to check if you can jump
	public LayerMask groundMask; //layermask for ground/planet


	Vector3 velocity;
	Vector3 gravitationalVelocity;
	InstagramInfluencer influencer;

	Rigidbody rb;

	void Start()
	{
		influencer = GetComponent<InstagramInfluencer>();
		rb = GetComponent<Rigidbody>();
		velocity = Vector3.zero;
		//gravitationalVelocity = Vector3.zero;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		//Movement();
	}

	void Jumping()
	{ 

	}


	private void FixedUpdate()
	{
		UpdateGravitationalVelocity(influencer.GetAttracted(), UniverseConstants.TimeDilation);
		rb.rotation = Quaternion.FromToRotation(transform.up, -gravitationalVelocity.normalized) * rb.rotation;

		rb.AddForce(gravitationalVelocity, ForceMode.Acceleration);
	}


	public void UpdateGravitationalVelocity(Vector3 acceleration, float timeDilation)
	{
		gravitationalVelocity = acceleration * timeDilation;
	}
}
