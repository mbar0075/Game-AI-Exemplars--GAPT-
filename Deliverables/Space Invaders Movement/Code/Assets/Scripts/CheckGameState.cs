using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGameState : MonoBehaviour
{
    GameObject player;
    GameObject[] invaders;

    //denoting of all the invaders have been killed
    bool allInvadersKilled = true;

    void Start()
    {
        //retrieivng the player and set of invaders
        invaders = GameObject.FindGameObjectsWithTag("Invader");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //if the player is not active they have lost
        if (player.activeSelf == false)
        {
            Debug.Log("You have lost!");
            Time.timeScale = 0;
            Destroy(this);
        }

        //setting the allInvadersKilled variable accrodingly if none of them are active
        foreach (GameObject obj in invaders)
        {
            if (obj.activeSelf == true)
            {
                allInvadersKilled = false;
                break;
            }
        }

        //If all invaders are killed the plaeyr has won otherwise reset variable to default value
        if (allInvadersKilled == true)
        {
            Debug.Log("You have won!");
            Time.timeScale = 0;
            Destroy(this.gameObject);
        }
        else
        {
            allInvadersKilled = true;
        }

        //If at least one invader reaches a certain level close to the player, the player loses
        foreach (GameObject obj in invaders)
        {
            if (obj.transform.position.y <= player.transform.position.y + 1)
            {
                Debug.Log("You have lost!");
                Time.timeScale = 0;
                Destroy(player);
                Destroy(this.gameObject);
            }
        }
    }
}
