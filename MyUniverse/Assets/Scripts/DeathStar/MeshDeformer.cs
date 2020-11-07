using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    Mesh deformingMesh;
    Vector3[] originalVertices;
    Vector3[] displacedVertices;
    Vector3[] vertexVelocities;
    int[] removedVertices;
    // Start is called before the first frame update
    float max;
    float min;
    int counter;
    void Start()
    {
        min = Mathf.Infinity;
        max = 0;
        counter = 0;
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        vertexVelocities = new Vector3[originalVertices.Length];
        for (int i=0; i<originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddDeformingForce(Vector3 point, float force, float scale)
    {
        min = Mathf.Infinity;
        max = 0;

        point = transform.InverseTransformPoint(point);
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        vertexVelocities = new Vector3[originalVertices.Length];
        removedVertices = new int[originalVertices.Length];
        

        for (int i=0; i<originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }

        for (int i=0; i<displacedVertices.Length; i++)
        {
            MinMaxDistance(i, point);
        }

        for (int i=0; i<displacedVertices.Length; i++)
        {
            AddForceVertex(i, force, point, scale);
        }

        for (int i = 0; i < displacedVertices.Length; i++)
        {
            UpdateVertices(i);
        }
        
        RemovePoints(scale);
        MeshCollider meshCollider = GetComponent<MeshCollider>(); //
        meshCollider.sharedMesh = deformingMesh; //
        meshCollider.convex = false;
        meshCollider.convex=true;
        deformingMesh.vertices = displacedVertices;//
        deformingMesh.RecalculateNormals();//




    }

    void AddForceVertex(int i, float force, Vector3 point, float scale)
    {
        float distance = Vector3.Distance(point, displacedVertices[i]);
        float diff = max-min;
        float lowerBound = diff*.2f+min;
        float upperBound = diff*.9f+min;;
        if (lowerBound < distance && distance<upperBound)
        {
            return;
        }
        
        removedVertices[counter] = i;
        counter += 1;
        
        Vector3 pointToVertex = displacedVertices[i] - point;
        pointToVertex *= scale;
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }

    void UpdateVertices(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        displacedVertices[i] += velocity * Time.deltaTime;
        vertexVelocities[i] += velocity*Time.deltaTime;
    }

    void RemovePoints(float sizeOfObj)//removes points that are far from center of object
    {

        
        //Vector3 center = transform.GetChild(0).position;
        //int[] removedVertices = new int[displacedVertices.Length];
        int[] oldTriangles = deformingMesh.triangles;

        /*for (int i=0; i<displacedVertices.Length; i++)
        {
            if (Vector3.Distance(center, displacedVertices[i]) > max+min)
            {
                removedVertices[counter] = i;
                counter += 1;
            }
        }*/
        int[] newTriangles = new int[oldTriangles.Length];

        int j = 0;
        for (int i=0; i<oldTriangles.Length; i+=3)
        {
            bool skip = false;
            for (int k=0; k<removedVertices.Length; k++)
            {
                if (removedVertices[k]==oldTriangles[i] || removedVertices[k] == oldTriangles[i+1]|| removedVertices[k] == oldTriangles[i+2])
                {
                    skip = true;
                    break;
                }
            }
            if (skip == true)
            {
                continue;
            }
            newTriangles[j] = oldTriangles[i];
            newTriangles[j + 1] = oldTriangles[i + 1];
            newTriangles[j + 2] = oldTriangles[i + 2];
            j += 3;
        }

        deformingMesh.triangles = newTriangles;
    }

    void MinMaxDistance(int i, Vector3 point) {
        float distance = Vector3.Distance(point, displacedVertices[i]);

        if (max<distance) {
            max=distance;
        }
        if (min>distance){
            min=distance;
        }
    }
}
