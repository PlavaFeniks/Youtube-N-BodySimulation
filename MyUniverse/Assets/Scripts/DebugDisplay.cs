using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugDisplay : MonoBehaviour
{
    Attractor[] attractors;
    public bool isOff;
    public int numSteps = 1;

    public float customTimeDilation;
    public bool useUniverseTimeDilation;

    public Attractor referenceObject;
    public bool useReferenceObject;

    private float timeDilation;
    private void OnValidate()
    {
        if (useUniverseTimeDilation)
        {
            timeDilation = UniverseConstants.TimeDilation;
        }
        else
        {
            timeDilation = customTimeDilation;
        }
    }

    public void Run()
    {

        if (!Application.isPlaying && !isOff)
        {
            DrawOrbits();
        }
        if (isOff)
        {
            HideOrbits();

        }
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            HideOrbits();
        }
    }

    private void Update()
    {
        if(!Application.isPlaying && !isOff)
        {
            DrawOrbits();
        }
        if (!Application.isPlaying && isOff)
        {
            HideOrbits();
        }
    }

    /*void FixedUpdate()
    {
        if (Application.isPlaying && !useReferenceObject)
        {
            attractors = FindObjectsOfType<Attractor>();
            for (int i=0; i<attractors.Length; i++)
            {
                LineRenderer lineRen = attractors[i].GetComponentInChildren<LineRenderer>();
                if (lineRen.positionCount == 0)
                {
                    return;
                }
                Vector3[] newPoints = new Vector3[lineRen.positionCount - 1];
                for (int j=0; j<lineRen.positionCount-1; j++)
                {
                    newPoints[j] = lineRen.GetPosition(j+1);
                }
                lineRen.SetPositions(newPoints);
                
            }
        }

    }*/


    void DrawOrbits()
    {
        attractors = FindObjectsOfType<Attractor>();
        //initialize
        Bodies[] bodies = new Bodies[attractors.Length];
        Vector3[][] points = new Vector3[bodies.Length][];
        int referenceIndex = 0;
        Vector3 referencePositionStart = Vector3.zero;

        for (int i=0; i<bodies.Length; i++)
        {
            bodies[i] = new Bodies(attractors[i]);
            points[i] = new Vector3[numSteps];
            
            //find reference
            if (attractors[i] == referenceObject && useReferenceObject)
            {
                referenceIndex = i;
                referencePositionStart = attractors[i].transform.position;
            }
        }


        //getpoints
        for (int step=0; step<numSteps; step++)
        {
            Vector3 referencePosition = Vector3.zero;
            if (useReferenceObject)
            {
                referencePosition = bodies[referenceIndex].position;
            }
           for (int i = 0; i < attractors.Length; i++)
           {
                bodies[i].velocity += Attract(i, bodies) * timeDilation;
           }
           for (int i = 0; i < attractors.Length; i++)
           {
                Vector3 newPosition = bodies[i].position + bodies[i].velocity * timeDilation;
                bodies[i].position = newPosition;
                if (useReferenceObject)
                {
                    newPosition -= referencePosition - referencePositionStart;
                    if (i==referenceIndex)
                    {
                        newPosition = referencePositionStart;
                    }
                }

                points[i][step] = newPosition;
           }

        }
        //drawpoints
        for (int i = 0; i<attractors.Length; i++)
        {
            Color pathColour = attractors[i].gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color;
            LineRenderer lineRen = attractors[i].GetComponentInChildren<LineRenderer>();
            lineRen.enabled = true;
            lineRen.positionCount = points[i].Length;
            lineRen.SetPositions (points[i]);
            lineRen.startColor = pathColour;
            lineRen.endColor = pathColour;
            lineRen.widthMultiplier = 100;

            

        }
    }

    void HideOrbits()
    {
        attractors = FindObjectsOfType<Attractor>();
        for (int i=0; i<attractors.Length; i++)
        {
            var lineRen = attractors[i].gameObject.GetComponentInChildren<LineRenderer>();
            lineRen.positionCount = 0; //this removes all points foreach line renderer
        }
    }

    Vector3 Attract(int indexOfReference, Bodies[] bodies)
    {
        Vector3 velocity = Vector3.zero;
        Bodies attractor = bodies[indexOfReference];
        for (int attractToIndex = 0; attractToIndex < bodies.Length; attractToIndex++)
        {
            if (attractToIndex != indexOfReference)
            {
                Bodies attractTo = bodies[attractToIndex];
                float mass = attractTo.mass;
                Vector3 direction = attractTo.position - attractor.position;
                float distance = direction.magnitude;

                float forceMagnitude = UniverseConstants.NewtonsGravityConstant * (mass) / Mathf.Pow(distance, 2);
                velocity += direction.normalized * forceMagnitude;
            }

        }
        return velocity;
    }

    class Bodies
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public Bodies(Attractor attractor)
        {
            position = attractor.transform.position;
            velocity = attractor.initialVeloctiy;
            mass = attractor.mass;
        }
    }

}
