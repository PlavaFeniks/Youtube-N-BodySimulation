using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
//It is affect by the gravity of attractors however it has no influence on other objects velocity
public class InstagramInfluencer : MonoBehaviour
{
	Rigidbody rb;
	public Vector3 velocity { get; private set; }
	public Attractor Planet { get; private set; }

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
	}

    public Vector3 GetAttracted()
    {
		velocity = GetForce(UniverseSim.Attractors);
		return velocity;
    }

	Vector3 GetForce(List<Attractor> attractors)
	{
		Vector3 newVelocity = Vector3.zero;
		Attractor closestPlanet = attractors[0];
		float minDistance = float.MaxValue;
		for (int attractToIndex = 0; attractToIndex < attractors.Count; attractToIndex++)
		{
			Attractor attractTo = attractors[attractToIndex];
			float mass = attractTo.mass;//attractor.Rigidbody().mass * 
			Vector3 direction = attractTo.Position() - rb.position;
			
			float distance = direction.magnitude;

			float forceMagnitude = UniverseConstants.NewtonsGravityConstant * (mass) / Mathf.Pow(distance, 2);
			
			newVelocity += direction.normalized * forceMagnitude;
			if (minDistance>distance)
			{
				closestPlanet = attractTo;
			}
		}
		Planet = closestPlanet;
		return newVelocity;
	}
}
