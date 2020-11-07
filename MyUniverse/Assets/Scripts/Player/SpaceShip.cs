using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
	//cleanup var
	//set break speed var
	//set max speed var

	public Camera cam; //very important goPro
	public Vector2 mouseSensitivity;

	[Header("Movement")]
	public float startingSpeed;
	public float moveSpeed;
	public float breakStrength;
	public float verticalMoveSpeed;
	public float rotationalSpeed;


	[Header("Parking")]
	//can be used when parking to hold it down
	public float stickToGroundForce; //minor force to help stick to the ground when not jumping, "Should" be replaced with surface gravity
	public Transform groundCheck; //given to empty object
	public float groundDistance; //distance to ground to check if you can jump
	public LayerMask groundMask; //layermask for ground/planet


	[HideInInspector]
	public bool movementActive = true;

	//local vars
	float mouseX;
	float mouseY;
	bool brake;
	Vector3 tempVelocity;
	Vector3 velocity;
	Vector3 gravitationalVelocity;
	Rigidbody rb;
	InstagramInfluencer influencer;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		influencer = GetComponent<InstagramInfluencer>();
		mouseX = 0;
		mouseY = 0;
		velocity = Vector3.zero;
		gravitationalVelocity = Vector3.zero;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		brake = false;
		tempVelocity = Vector3.zero;
	}

	void Update()
	{
		if (movementActive) {
			Looking();
			Movement();
		}
	}

	 
	void Movement()
	{
		if (Input.GetAxisRaw("Vertical") != 0)
		{
			tempVelocity = transform.forward * moveSpeed;
		}

		if (Input.GetKey(KeyCode.LeftShift))
		{
			brake = true;
		}
		else
		{
			brake = false;
		}
		velocity = tempVelocity + VerticalMovement();
	}

	Vector3 VerticalMovement()
	{
		if (Input.GetButton("Jump"))
		{
			return transform.up * verticalMoveSpeed;
		}
		else if (Input.GetKey(KeyCode.LeftControl)) {
			return (-transform.up) * verticalMoveSpeed;
		}
		return Vector3.zero;
	}



	void Looking()
	{//looking
		mouseX = Input.GetAxis("Mouse X") * mouseSensitivity.x * Time.deltaTime;
		mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity.y * Time.deltaTime;
		float zRotation = -Input.GetAxisRaw("Horizontal")* rotationalSpeed * Time.deltaTime;



		transform.Rotate(new Vector3(mouseY, mouseX, zRotation), Space.Self);

	}

	bool Landing()
	{
		bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
		if (isGrounded)
		{
			rb.rotation = Quaternion.FromToRotation(transform.up, -gravitationalVelocity.normalized) * rb.rotation;
			Debug.Log("ON GROUND");
			return true;
		}
		return false;
	}

	

	private void FixedUpdate()
	{
		if (brake)
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			tempVelocity = Vector3.zero;
		}
		else
		{
			rb.AddForce(velocity, ForceMode.Acceleration);
		}
		Vector3 acceleration = influencer.GetAttracted();
		if (Landing()) {
		}
		UpdateGravitationalVelocity(acceleration, UniverseConstants.TimeDilation);
		rb.AddForce(gravitationalVelocity, ForceMode.Acceleration);
		//rb.MovePosition(rb.position + velocity * Time.deltaTime);
	}


	public void UpdateGravitationalVelocity(Vector3 acceleration, float timeDilation)
	{
		gravitationalVelocity = acceleration * timeDilation;
	}
}
