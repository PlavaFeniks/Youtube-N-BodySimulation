using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//runs phyics for universe, specifically gravity

public class UniverseSim : MonoBehaviour
{
	public static List<Attractor> Attractors;
	public Camera cam;
	public Attractor referenceObject;


	void Awake() {
	   //Attractors = new List<Attractor>();
	}

	private void Update()
	{
		cam.transform.parent = referenceObject.transform;
	}

	void FixedUpdate() {
		NewtonsGravity();
	}
   
	void NewtonsGravity()
	{
		
		for (int referenceIndex=0; referenceIndex < Attractors.Count; referenceIndex++)// reference frame
		{
			Vector3 velocity = Vector3.zero;
			velocity += Attract(referenceIndex);
			Attractors[referenceIndex].UpdateVelocity(velocity, UniverseConstants.TimeDilation);
		}

		foreach(Attractor attractor in Attractors)
		{
			attractor.UpdatePosition(UniverseConstants.TimeDilation);
		}
	}
   
	/*could consider running calculations on gpu because if it is intensive;
   only thing that would be difficult is organizing forces, normalizing the direction, and obtaining the magnitude of the direction
   */
	Vector3 Attract(int indexOfReference) {
		Vector3 velocity = Vector3.zero;
		Attractor attractor = Attractors[indexOfReference];
		for(int attractToIndex=0; attractToIndex<Attractors.Count; attractToIndex++)
		{
			if (attractToIndex != indexOfReference)
			{
			Attractor attractTo = Attractors[attractToIndex];
			float mass = attractTo.mass;//attractor.Rigidbody().mass * 
			Vector3 direction = attractTo.Position() - attractor.Position();
			float distance = direction.magnitude;

			float forceMagnitude = UniverseConstants.NewtonsGravityConstant * (mass) / Mathf.Pow(distance, 2);
			velocity += direction.normalized * forceMagnitude;
			}

		}
		return velocity;
    }	





	
}