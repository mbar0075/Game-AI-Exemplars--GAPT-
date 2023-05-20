using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderShooting : MonoBehaviour
{
    //shootingPoint denotes the empty object at which the bullet will spawn
    public GameObject shootingPoint;

    //bullet is the prefab that will be shot by the player
    public GameObject bullet;

    //fireRate denotes the amount of secodns that need to pass for another shot to be fired
    private float fireRate;

    //nextFire denotes the relative time until the next shot can be fired 
    private float nextFire;

    void Start()
    {
        //sets teh firerate to a value betwen 5 and 10  for each invader
        fireRate = Random.Range(5f, 10f);
    }
    // Update is called once per frame
    void Update()
    {
        //given the required time has passed ashot is passed 
        if (Time.time > nextFire)
        {
            //used to stop the invaders from firing all at once upon game start
            if (nextFire != 0)
            {
                Instantiate(bullet, shootingPoint.transform.position, Quaternion.identity);
            }

            //updating the time at which the next shot will be fired
            nextFire = Time.time + fireRate;
        }
    }
}
