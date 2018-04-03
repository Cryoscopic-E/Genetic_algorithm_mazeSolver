using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    //0 move forward
    //1 turn left
    //2 turn right

    //Bot has to make 1 choiche: when is colliding with a wall turn by an angle
    public int chromosomeLength = 1;

    //Reference to the eye game object where to cast the spherecast from, for check wall collision
    public Transform eyes;

    public float speed = 5f;
    

    //TODO hide to inspector
    public float distanceWalked = 0f;
    //TODO hide to inspector
    public bool goalReached;

    public Chromosome chromosome;


    private bool hitWall;

    private Vector3 startPoint;

    //Initialize the brain by creating new chromosome and set the starting rotation
    public void Init(Vector3 startPosition)
    {
        chromosome = new Chromosome(chromosomeLength, 360);
        transform.Rotate(0f, chromosome.GetGene(0), 0f);
        goalReached = false;
        distanceWalked = 0f;
        startPoint = startPosition;
    }

    private void Update()
    {
        if (goalReached)
        {
            return;
        }
        hitWall = false;
        RaycastHit hit;
        if(Physics.SphereCast(eyes.position, 0.1f,transform.forward, out hit, .2f))
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("wall"))
                {
                    hitWall = true;
                }
            }
        }
        
        float r = 0f;
        if (hitWall)
        {
            r = chromosome.GetGene(0);
        }
        transform.Translate(0, 0, speed * Time.deltaTime);
        transform.Rotate(0f, r, 0f);
        distanceWalked = Vector3.Distance(transform.position, startPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("goal"))
        {
            goalReached = true;
            //Debug.Log("GOAL REACHED!");
        }
    }
}
