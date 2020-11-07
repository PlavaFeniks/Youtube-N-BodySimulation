using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//switch camera when in ship or near death star
//have jetpack mode where affected by gravity wholey
//have non-jetpack mode where affect by gravity away with no control, and affected by surface G'

public class SpaceSuitControls : MonoBehaviour
{

	public float mouseSensitivity;
	public float moveSpeed;
	public float jumpForce;

	public float stickToGroundForce; //minor force to help stick to the ground when not jumping, "Should" be replaced with surface gravity
	public Transform groundCheck; //given to empty object
	public float groundDistance; //distance to ground to check if you can jump
	public LayerMask groundMask; //layermask for ground/planet

	public Camera cam; //very important goPro
	[HideInInspector]
	public bool movementActive = true;

	float mouseX;
	float mouseY;
	Vector3 velocity;
	Vector3 gravitationalVelocity;
	InstagramInfluencer influencer;

	Rigidbody rb;
	

	void Start()
	{
		influencer = GetComponent<InstagramInfluencer>();
		rb = GetComponent<Rigidbody>();
		mouseX = 0;
		mouseY = 0;
		velocity = Vector3.zero;
		//gravitationalVelocity = Vector3.zero;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		if (movementActive) {
			Movement();
		}
	}


	void Movement()
	{
		//Looking script
		Looking();

		//move
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		velocity = transform.TransformDirection(input.normalized) * moveSpeed;

		Jumping();

	}


	void Looking()
	{//looking
		mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
		mouseY -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;
		mouseY = Mathf.Clamp(mouseY, -90f, 90f);
		cam.transform.localEulerAngles = Vector3.right * mouseY;
		transform.Rotate(Vector3.up * mouseX, Space.Self);

	}

	void Jumping()
	{ //have to revise "up" direction 
		bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
		if (isGrounded)
		{
			if (Input.GetButtonDown("Jump"))
			{
				rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
				isGrounded = false;
			}
			else
			{
				//rb.AddForce(-transform.up * stickToGroundForce, ForceMode.VelocityChange);
			}
		}
	}


	private void FixedUpdate()
	{
		UpdateGravitationalVelocity(influencer.GetAttracted(), UniverseConstants.TimeDilation);

		rb.AddForce(gravitationalVelocity, ForceMode.Acceleration);
		rb.rotation = Quaternion.FromToRotation(transform.up, -gravitationalVelocity.normalized) * rb.rotation;
		
		rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
	}


	public void UpdateGravitationalVelocity(Vector3 acceleration, float timeDilation)
	{
		gravitationalVelocity = acceleration * timeDilation;
	}

	public void ActivateCamera()
	{

	}
}