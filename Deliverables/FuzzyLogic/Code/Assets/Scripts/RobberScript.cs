using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberScript : MonoBehaviour
{
    //Variables to hold min and max width
    [SerializeField] private int minwidth,maxwidth;
    //Variables to hold min and max height
    [SerializeField] private int minheight,maxheight;
    //Variable to hold the Z axis position
    [SerializeField] private int zAxis;
    //Variable to hold the moveSpeed
    [SerializeField] private float moveSpeed;
    //Variable to hold the target position for the object to move to
    private Vector3 targetposition;
    //Variable which holds the Object's rigidbody2d
    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start(){
        //Retrieving rigidbody2D component
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {   
        //Calling the Move in Random Directions Function
        MoveInRandomDirections();
    }

    //Method which enables Game Object to move in random directions
    private void MoveInRandomDirections(){
        //Calculating new move position via Move Towards function
        Vector3 moveposition=Vector3.MoveTowards(transform.position,targetposition,moveSpeed * Time.deltaTime);

        //Flipping sprite depending on the moveposition
        if (moveposition.x-transform.position.x >= 0.01f )
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (moveposition.x-transform.position.x < 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        
        //Movement
        //Checking whether object reached target position, if not will move the object to the new moveposition,
        //if yes, will proceed to generate a new target position
        if(transform.position!=targetposition){
            transform.position=moveposition;
            
        }
        else{
            targetposition= new Vector3(Random.Range(minwidth,maxwidth),Random.Range(minheight,maxheight),zAxis);
        }
    }

}
