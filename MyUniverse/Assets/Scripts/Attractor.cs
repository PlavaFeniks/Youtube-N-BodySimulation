using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//given to anything that wants gravity

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class Attractor : MonoBehaviour {
	
	public string nameOfBody;
	public Vector3 initialVeloctiy;
	public float radius;
	public float mass{ get; private set; }
	
	public float surfaceG;



	private Rigidbody rb;
	Vector3 velocity;

	//gets components and sets default values
	void Awake() {
		rb = GetComponent<Rigidbody>();
		
		velocity = initialVeloctiy;
	}
	
	//ran when script
	void OnValidate (){
		if (radius <=0)
		{
			radius = 1;
		}
		mass = surfaceG * radius * radius / UniverseConstants.NewtonsGravityConstant;
		gameObject.name = nameOfBody;
		transform.localScale = new Vector3(radius, radius, radius);
	}
	
	void OnEnable() {
		if (UniverseSim.Attractors is null)
		{
			UniverseSim.Attractors = new List<Attractor>();
		}
		UniverseSim.Attractors.Add(this);
		Debug.Log(nameOfBody + ": connected");
	}
	
	void OnDisable() {
		UniverseSim.Attractors.Remove(this);
	}
	
	void Start() {
		//AddForce(initialVeloctiy);
		rb.mass = mass;
		rb.mass = surfaceG* radius *radius / UniverseConstants.NewtonsGravityConstant;
	}
	
	public Rigidbody Rigidbody() {
		return rb;
	}
	
	public Vector3 Position() {
		return rb.position;
	}
	
	public Vector3 Velocity()
	{
		return velocity;
	}

	//used if you want to use Unity's force system
	public void AddForce(Vector3 force) {
		rb.AddForce(force);
	}

	public void UpdateVelocity(Vector3 acceleration, float timeDilation)
	{
		velocity += acceleration * timeDilation;
	}

	public void UpdatePosition(float timeDilation)
	{
		rb.MovePosition(rb.position + velocity * timeDilation);
	}
	
	
	
}