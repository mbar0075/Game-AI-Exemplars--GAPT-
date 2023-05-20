using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    //Declaring timer per spawn and enemy game object
    [SerializeField] private float timer = 5f, timePerSpawn = 10f;
    [SerializeField] private GameObject enemy;

    void Update() {
        //Decrease timer by Time.deltaTime
        timer -= Time.deltaTime;
        //If timer is smaller or equal to zero, Instantiate an enemy and reset timer
        if (timer <= 0f) {
            Instantiate(enemy, transform.position, Quaternion.identity);
            timer = timePerSpawn;
        }
    }
}
