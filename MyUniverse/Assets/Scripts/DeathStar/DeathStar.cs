using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStar : MonoBehaviour
{
    //controls for death star
    public Attractor orbitingPlanet;
    Rigidbody rb;
    MouseMeshDeformer msDf;
    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        msDf = GetComponent<MouseMeshDeformer>();
    }
    bool alreadyClicked = false;
    private void FixedUpdate()
    {
        if (orbitingPlanet == null)
        {
            return;
        }
        Vector3 planetsPosition = orbitingPlanet.Position();//
        Vector3 beamDirection = (planetsPosition - transform.position).normalized;//
        rb.rotation = Quaternion.FromToRotation(transform.forward, beamDirection) * rb.rotation; //




        if (Input.GetKey("h") && alreadyClicked==false)
        {
            msDf.HandleInput(rb.position, transform.forward, orbitingPlanet.radius);
            alreadyClicked=true;
        }
    }
}
