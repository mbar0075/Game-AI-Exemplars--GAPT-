using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceScript : MonoBehaviour
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
    //Variable to hold the Closest Robber Distance
    private float closestRobberDistance;
    //Variable to hold the goToBank Value/Percentage
    public float goToBankValue;
    //Variable to hold the patrol Value/Percentage
    public float patrolValue;
    //Variable to hold the chaseRobber Value/Percentage
    public float chaseRobberValue;
    //Boolean variable used as a flag, to determine whether random position was set
    private bool randomPosSet=false;
    //Variable to hold the object's current state
    public string state="";
    //Variables to hold the fuzzy Curves, for the respective states
    public AnimationCurve chaseRobber; 
    public AnimationCurve goToBank; 
    public AnimationCurve patrol; 


    // Update is called once per frame
    private void Update()
    {   
        //Calling method DetermineState()
        DetermineState();
        
    }

    //Method which determines the current state, based on the degree of membership for each fuzzy set
    private void DetermineState(){
        //Retrieving the Closest Robber distance
        closestRobberDistance=GetClosestRobber();
        //Retrieving the degree for each fuzzy set, based on fuzzy curve and closest robber distance
        chaseRobberValue = chaseRobber.Evaluate(closestRobberDistance); 
        goToBankValue = goToBank.Evaluate(closestRobberDistance); 
        patrolValue = patrol.Evaluate(closestRobberDistance); 

        //Calculating the maximum value from all the fuzzy values
        float maxValue=Mathf.Max(chaseRobberValue, Mathf.Max(goToBankValue, patrolValue));
        //Based on which fuzzy value is largest, the relevant Target Position is set
        if(maxValue==chaseRobberValue){
            SetTargetPosToClosestRobber();
        }
        if(maxValue==goToBankValue){
            SetTargetPosToBank();
        }
        if(maxValue==patrolValue){
            SetTargetPosToRandomPos();
        }
        //Moving to Target Position
        MoveToTargetPosition();
    }

    //Method which the returns, the distance of the Closest Robber
    private float GetClosestRobber(){
        //Setting smallestDistance to a large amount
        float smallestDist = Mathf.Infinity;
        //Retrieving, all game objects with the Robber tag
        GameObject [] robbers = GameObject.FindGameObjectsWithTag("Robber");
        //Looping through all the robbers
        for(int i=0;i<robbers.Length;i++){
            //Calculating the distance, and checking whether it is smaller than the smallest distance
            float dist = Vector3.Distance(robbers[i].transform.position, transform.position);
            if(dist<smallestDist){
                smallestDist=dist;
            }
        }
        //Returning smallest distance
        return smallestDist;
    }


    //Method which sets the Target Position to a Random Positionn
    private void SetTargetPosToRandomPos(){
        //Checking if the randomPosSet flag is not set, if so will proceed to set the targetposition to a new random position
        if(randomPosSet==false){
            targetposition= new Vector3(Random.Range(minwidth/2,maxwidth/2),Random.Range(minheight/2,maxheight/2),zAxis);
            randomPosSet=true;
        }
        //Checking whether the current object reached the current position, and if so will proceed to set the targetposition to a new random position
        if(transform.position==targetposition){
            targetposition= new Vector3(Random.Range(minwidth/2,maxwidth/2),Random.Range(minheight/2,maxheight/2),zAxis);
        }
        //Changing state to "Patrolling"
        state="Patrolling";
    }

    //Method which sets the Target Position to Bank Position
    private void SetTargetPosToBank(){
        //Setting the targetposition, to the Bank's position
        targetposition= new Vector3((maxwidth+minwidth)/2,(maxheight+minheight),zAxis);
        //Changing state to "Going to Bank"
        state="Going to Bank";
        //Setting randomPosSet Flag to false
        randomPosSet=false;
    }

    //Method which sets the Target Position to Closest Robber Position
    private void SetTargetPosToClosestRobber(){
        //Setting smallestDistance to a large amount
        float smallestDist = Mathf.Infinity;
        //Retrieving, all game objects with the Robber tag
        GameObject [] robbers = GameObject.FindGameObjectsWithTag("Robber");
        for(int i=0;i<robbers.Length;i++){
            //Calculating the distance, and checking whether it is smaller than the smallest distance
            float dist = Vector3.Distance(robbers[i].transform.position, transform.position);
            if(dist<smallestDist){
                smallestDist=dist;
                //Changing the targetposition to the closest's robber position
                targetposition=robbers[i].transform.position;
            }
        }
        //Changing state to "Chasing Robber"
        state="Chasing Robber";
        //Setting randomPosSet Flag to false
        randomPosSet=false;
    }


    //Method, which moves the Game Object to the Target Position
    private void MoveToTargetPosition(){
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
        //Setting transform.position to moveposition
        transform.position=moveposition;
    }

    //Method which checks, whether there were any on Trigger Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checking, whether Police collided with any Robbers, if so will proceed to Destroy Robber Game Objects
        if (collision.gameObject.tag == "Robber")
        {
            Destroy(collision.gameObject);
        }
    }

}
