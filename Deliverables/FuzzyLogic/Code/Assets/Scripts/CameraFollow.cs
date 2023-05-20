using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Variable to hold the cameraSmoothness
    [SerializeField] private float cameraSmoothness;

    // Update is called once per frame
    private void Update()
    {
        //Calling Follow Police Method
        FollowPolice();
    }

    //Method which allows the Camera to follow the Police
    private void FollowPolice(){
        //Retrieving the game object with the Police Tag
        GameObject police = GameObject.FindWithTag("Police");
        //Checking that police is not null, and if so, will update the Camera position
        if(police!= null){
        transform.position =Vector3.Lerp(transform.position, (police.transform.position + new Vector3(0,0,transform.position.z)), cameraSmoothness*Time.fixedDeltaTime);
        }
    }
}
