﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    //0 move forward
    //1 turn left
    //2 turn right

    //Bot has to make 2 choiches whether is colliding with a wall or not
    public int chromosomeLength = 1;

    //Reference to the eye game object where to cast the ray from, for check wall collision
    public Transform eyes;

    public LayerMask wallLayer;

    public float speed = 5f;
    

      //TODO hide to inspector
    public float distanceWalked = 0f;

    public Chromosome chromosome;

    private bool hitWall;

    public bool goalReached;

    private Vector3 startPoint;


    public void Init(Vector3 startPosition)
    {
        chromosome = new Chromosome(chromosomeLength, 360);
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
            Debug.Log("GOAL REACHED!");
        }
    }
}