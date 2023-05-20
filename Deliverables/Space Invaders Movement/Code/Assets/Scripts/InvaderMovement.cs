using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderMovement : MonoBehaviour
{
    //invader movement speed
    public float speed = 1;

    //invader down movement amount
    public int downMovement = 1;

    //invader steps taken in either side
    public int stepNumber = 100;

    //denotes wether to move left or right
    private bool moveRight = true;

    //counts the number of steps taken in either side
    private int stepCount = 0;

    GameObject[] invaders;

    void Start()
    {
        //Retrieving the set of invaders 
        invaders = GameObject.FindGameObjectsWithTag("Invader");
    }

    void FixedUpdate()
    {

        //When the amount of steps are done the invaders change direction and move downwards
        if (stepCount == stepNumber)
        {
            //Inverting the movement direction 
            moveRight = !moveRight;

            //Resetting stepCount variable 
            stepCount = 0;

            //Moving each invader downwards 
            foreach (GameObject obj in invaders)
            {
                obj.transform.position += this.transform.up * -downMovement * Time.deltaTime;
            }
        }

        //Moving each invader left or right depending on the moveRight bool variable
        foreach (GameObject obj in invaders)
        {
            if (moveRight)
            {
                obj.transform.position += this.transform.right * speed * Time.deltaTime;
            }
            else
            {
                obj.transform.position += this.transform.right * -speed * Time.deltaTime;
            }
        }

        //Incrementing stepCount for each step taken
        stepCount++;
    }
}
