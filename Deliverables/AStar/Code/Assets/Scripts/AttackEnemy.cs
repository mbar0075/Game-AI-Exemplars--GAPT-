using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour {
    //Declaring game objects and variables to be used
    private GameObject enemy;
    private GameObject [] enemies;
    public LineRenderer lineRenderer;
    [SerializeField] private float timeToShoot = 1f, distanceBetween = 4f;
    

    void Update() {
        //Find all game objects with tag enemy
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //Call the function to get the closest enemy and save it in enemy
        enemy = GetClosestEnemy(enemies);
        //If the enemy exists, calculate the distance between it and the current gameobject,
        //if it is less or equal to the variable distance between, call the look at target and shoot laser functions
        if (enemy != null) {
            if (Vector2.Distance(transform.position, enemy.transform.position) <= distanceBetween) {
                LookAtTarget();
                ShootLaser();
            }
        }
    }

    private void LookAtTarget() {
        //Set direction to the vector difference between location and the game object's position
        Vector3 direction = enemy.transform.position - transform.position;
        //Normalise direction
        direction.Normalize();
        //Calculate the rotation and set the game object's rotation to the Calculated value
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation -= 90;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    void ShootLaser() {
        //Display a line between the game object and its target
        Debug.DrawLine(transform.position, enemy.transform.position, Color.red);
        //Decrease timeToShoot by Time.deltaTime, and if it is less or equal to zero, destroy the enemy and reset timeToShoot
        timeToShoot -= Time.deltaTime;
        if ((timeToShoot <= 0f)) {
            Destroy(enemy);
            timeToShoot = 1f;
        }
    }

    GameObject GetClosestEnemy (GameObject[] enemies) {
        //Declaring variables to be used
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        //For each gameobject in the passed enemies array, calculate the distance between it and the current game object,
        //If its distance squared is less than closestDistanceSqr, set closestDistanceSqr to the new calculated distance and
        //bestTarget to the current target
        foreach(GameObject potentialTarget in enemies) {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }
}
