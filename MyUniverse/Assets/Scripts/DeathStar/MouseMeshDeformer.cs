using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMeshDeformer : MonoBehaviour
{
    // Start is called before the first frame update
    public float force = 10f;
    public float forceOffset = .01f;

    void Start()
    {

    }

    bool alreadyClicked = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("h") && alreadyClicked==false)
        {
            HandleInput(transform.position, transform.forward, 1);
            alreadyClicked=true;
        }
    }

    public void HandleInput(Vector3 position, Vector3 direction, float radius)
    {
        Ray inputRay = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer)
            {
                Vector3 point = hit.point;
                point += hit.normal*forceOffset;
                deformer.AddDeformingForce(point, force, radius);
            }
        }

    }

   /*public void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer)
            {
                Vector3 point = hit.point;
                Vector3 pointForce = point + hit.normal * forceOffset;
                deformer.AddDeformingForce(pointForce, point, force);
            }
        }

    } */
}
