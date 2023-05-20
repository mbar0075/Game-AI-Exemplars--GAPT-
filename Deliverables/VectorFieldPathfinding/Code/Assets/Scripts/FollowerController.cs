using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
    //Variable which holds an Instance of the Vectors Class
    public Vectors Vector;
    //Variable which holds an Instance of the FlowFieldGrid Class
    public FlowFieldGrid flowField;
    //Variable which holds the Object's rigidbody2d
    private Rigidbody2D rb;
    //Variable which holds the Object's Transform position
    private Transform Position;
    //Variable which holds the Object's move speed
    [SerializeField] private float moveSpeed;

    //Start method
    private void Start(){
        //Retrieving components
        rb=GetComponent<Rigidbody2D>();
        Position=GetComponent<Transform>();
    }

    //Fixed Update method
    private void FixedUpdate()
    {   
        //Object movement via FlowFieldGrid
        //If Field has not been generated then return
        if (flowField.mouseclicked == false) { 
            return; 
        }
        //Else field has been generated, and object can move
        else{
            //Retrieving the Cell where the Object is at on the Flow Field Grid
            Cell currentCellPos = flowField.GetCellFromVector3(Position.position);
            //Retrieving Vector3 move direction from currentCellPos
            Vector3 moveDirection = new Vector3(currentCellPos.bestDirection.vector.x, currentCellPos.bestDirection.vector.y, 0);
            //Multiplying vector by move speed
            rb.velocity = moveDirection * moveSpeed;
        }

        //Flipping sprite, if object is moving in tthe opposite direction, via rb.velocity.x
        if (rb.velocity.x >= 0.01f )
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rb.velocity.x < 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
