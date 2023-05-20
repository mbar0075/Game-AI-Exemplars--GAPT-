using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //shootingPoint denotes the empty object at which the bullet will spawn
    public GameObject shootingPoint;

    //bullet is the prefab that will be shot by the player
    public GameObject bullet;

    //player health
    public int health = 3;

    //player movement speed
    public float speed = 5;

    //fireRate denotes the amount of secodns that need to pass for another shot to be fired
    public float fireRate = 1;

    //nextFire denotes the relative time until the next shot can be fired 
    private float nextFire;

    void FixedUpdate()
    {
        //Player movement 
        //Checking that required key is pressed and player isn't off screen
        if (Input.GetKey(KeyCode.D) && this.transform.position.x <= 9.09)
        {
            //Moving player to the right
            this.transform.position += this.transform.right * speed * Time.deltaTime;
        }
        //Checking that required key is pressed and player isn't off screen
        else if (Input.GetKey(KeyCode.A) && this.transform.position.x >= -9.09)
        {
            //Moving player to the left
            this.transform.position += this.transform.right * -speed * Time.deltaTime;
        }

        //Checking that required key is pressed and making sure the required time has passed before next shot
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
        {
            //Incrementing timer
            nextFire = Time.time + fireRate;
            //Instantiate a bullet object
            Instantiate(bullet, shootingPoint.transform.position, Quaternion.identity);
        }
    }

    //Function to reduce player health on hit
    public void GetHit()
    {
        //Reducing current health by 1
        health -= 1;

        //Deactivaitng the player if their health drops to 0
        if (health == 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
