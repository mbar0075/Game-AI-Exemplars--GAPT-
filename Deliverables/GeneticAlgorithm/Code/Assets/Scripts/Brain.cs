using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    //Denote the number of values that compoe the DNA
    int DNALength = 14;
    //Denotes the maxValue allowed for each gene in the DNA
    int DNAMaxVal = 180;
    //DNA script object
    public DNA dna;
    //Game object denoting the view area
    public GameObject viewArea;
    //Explosion game object spawned on crash
    public GameObject explosionPrefab;
    //Agent rigidbody reference
    public Rigidbody2D rb; 

    //Boolean variables denoting what is being detected
    //Left/Right mars floor
    bool seeLeftGround = false;
    bool seeRightGround = false;
    //Up/Left/Right Death walls 
    bool seeLeftDeath = false;
    bool seeRightDeath = false;
    bool seeUpDeath = false;
    //Goal
    bool seeGoal = false;

    //Spawn position of the Agent
    Vector3 startPosition;

    //Time assigend to each generation used for reward calculation
    float fuelAmount = 0;
    //Time denoting the amount of time the agent has been alive
    public float timeAlive = 0;
    //Negative reward value
    public float crash = 0;
    //Positive reward value
    public float reachGoal = 0;
    //Used to calculate positive reward
    public float distanceToGoal = 0;
    //Denotes if the agent is alive
    bool alive = true;  
    
    //Max values for rotattion and thrust
    float maxRot = 45;
    int maxThrustPower = 2;

    //Bitshifting the index of the obstacle layer to get a bit mask 
    int ObstacleLayerMask = 1 << 8;
    //Specifiying the ray cast length
    public float RayCastLength = 1;

    //Method to initialize the lander
	public void Init(float time)
	{
        //Initializing DNA object
        dna = new DNA(DNALength,DNAMaxVal);
        
        //Assigning the fuel time 
        fuelAmount = time;
        
        //Choosing the spawn position of the Agent, making sure that they don't spawn above the landing zone
        if(Random.Range(0,2) == 0){
            this.transform.Translate(Random.Range(-7f,-3f),Random.Range(0f,3f),0);
        }
        else{
            this.transform.Translate(Random.Range(3f,7f),Random.Range(0f,3f),0);
        }
        startPosition = this.transform.position;

        //Retrieving the rigid body reference
        rb = this.GetComponent<Rigidbody2D>();
	}
    
    //Detects when the lander reacehs the goal or crashes
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Goal")
        {
            //Assigning the positive reward according to how fast the agent managed to land
            reachGoal += fuelAmount;
            //Incrementing the number of agents that reached the goal 
            PopulationManager.reachedGoal += 1;
            //Changing the rigid body type to static so as to ignore gravity - done for visual purposes
            rb.bodyType = RigidbodyType2D.Static;
            //Disabling the 2D collider - done for visual purposes
            this.GetComponent<Collider2D>().enabled = false;
        }
        else if(col.gameObject.tag == "LGround" || 
        	col.gameObject.tag == "RGround"||
        	col.gameObject.tag == "UDeath" ||
        	col.gameObject.tag == "LDeath" ||
        	col.gameObject.tag == "RDeath")
        {
            //Assigning the negative reward accordingly if the agent crashed before or after running out of fuel
            if(fuelAmount > 0)
            {
                crash += 1000;
            }
            else
            {
                crash += distanceToGoal;
            }

            //Instantiating the explosion - done for visual purposes
            Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
            //Deactivating the gameObject - done for visual purposes
            this.gameObject.SetActive(false);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //Do nothing if the lander dies or runs out of fuel  
        if(!alive || fuelAmount <= 0) return;

        //Denoting what the lander is seeing 
        seeLeftGround = false;
        seeRightGround = false;
        seeLeftDeath = false;
        seeRightDeath = false;
        seeUpDeath = false;
        seeGoal = false;
        
        //Looking Up
        Debug.DrawRay(viewArea.transform.position, viewArea.transform.up* 1.5f, Color.blue);
        //Looking Down
        Debug.DrawRay(viewArea.transform.position, -viewArea.transform.up* 1.0f, Color.red);
        //Looking Left
        Debug.DrawRay(viewArea.transform.position, -viewArea.transform.right* 1.0f, Color.yellow);
        //Looking Left Angled 
        Debug.DrawRay(viewArea.transform.position, ((-viewArea.transform.right + -viewArea.transform.up)/2) * 1.5f, Color.yellow);
        //Looking Right
        Debug.DrawRay(viewArea.transform.position, viewArea.transform.right* 1.0f, Color.green);
        //Looking Right Angled 
        Debug.DrawRay(viewArea.transform.position, ((viewArea.transform.right + -viewArea.transform.up)/2) * 1.5f, Color.green);

        //Checking upwards and setting the appropriate flags
        RaycastHit2D hit = Physics2D.Raycast(viewArea.transform.position, viewArea.transform.up, (float)(RayCastLength+0.5), ObstacleLayerMask);
        if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "UDeath")
            {
                seeUpDeath = true;
            }
        }

        //Checking downwards and setting the appropriate flags
		hit = Physics2D.Raycast(viewArea.transform.position, -viewArea.transform.up, RayCastLength*3, ObstacleLayerMask);
        if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "LGround")
            {
                seeLeftGround = true;
            }
            else if(hit.collider.gameObject.tag == "RGround"){
                seeRightGround = true;
            }
            else if(hit.collider.gameObject.tag == "Goal"){
                seeGoal = true;
            }
        }

        //Checking Left and setting the appropriate flags
		hit = Physics2D.Raycast(viewArea.transform.position, -viewArea.transform.right* 0.8f, RayCastLength, ObstacleLayerMask);
        if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "LGround")
            {
                seeLeftGround = true;
            }
            else if(hit.collider.gameObject.tag == "Goal"){
                seeGoal = true;
            }
            else if(hit.collider.gameObject.tag == "LDeath"){
                seeLeftDeath = true;
            }
        }

        //Checking Left Angled and setting the appropriate flags
		hit = Physics2D.Raycast(viewArea.transform.position, ((-viewArea.transform.right + -viewArea.transform.up)/2), RayCastLength,ObstacleLayerMask);
        if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "LGround")
            {
                seeLeftGround = true;
            }
            else if(hit.collider.gameObject.tag == "Goal"){
                seeGoal = true;
            }
        }

        //Checking Right and setting the appropriate flags
		hit = Physics2D.Raycast(viewArea.transform.position, viewArea.transform.right, RayCastLength, ObstacleLayerMask);
        if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "RGround"){
                seeRightGround = true;
            }
            else if(hit.collider.gameObject.tag == "Goal"){
                seeGoal = true;
            }
            else if(hit.collider.gameObject.tag == "RDeath"){
                seeRightDeath = true;
            }
        }

        //Checking Right Angled and setting the appropriate flags
		hit = Physics2D.Raycast(viewArea.transform.position, ((viewArea.transform.right + -viewArea.transform.up)/2), RayCastLength, ObstacleLayerMask);
        if (hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "RGround"){
                seeRightGround = true;
            }
            else if(hit.collider.gameObject.tag == "Goal"){
                seeGoal = true;
            }
        }

        //Updating the time alive
        timeAlive = PopulationManager.elapsed;
        fuelAmount -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        //Do nothing if the lander dies or runs out of fuel 
        if(!alive || fuelAmount <= 0) return;
        
        //Denoting the thrustPower and rotation
        float thrustPower = 0;
        float rotation = 0;

        //Calculating the agent distance from the goal
        distanceToGoal = Vector3.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Goal").transform.position);
        
        //Assigning the thrustPower and rotation values depending on what is being detected
        if(seeLeftGround)
        { 
            //Retrieving appropriate gene from the agent chromosome
            thrustPower = dna.GetGene(0)*distanceToGoal*Time.deltaTime;
            rotation -= dna.GetGene(1) * Time.deltaTime;
        }
        
        if(seeRightGround)
        {
            //Retrieving appropriate gene from the agent chromosome
        	thrustPower = dna.GetGene(2)*distanceToGoal*Time.deltaTime;
            rotation += dna.GetGene(3) * Time.deltaTime;
        }
        
        if(seeLeftDeath)
        {
            //Retrieving appropriate gene from the agent chromosome
        	thrustPower =  dna.GetGene(4)*distanceToGoal*Time.deltaTime;
            rotation -= dna.GetGene(5) * Time.deltaTime;
        }        
        
        if(seeRightDeath)
        {
            //Retrieving appropriate gene from the agent chromosome
        	thrustPower =  dna.GetGene(6)*distanceToGoal*Time.deltaTime;
            rotation += dna.GetGene(7) * Time.deltaTime;
        }
        
        if(seeUpDeath)
        {
            //Retrieving appropriate gene from the agent chromosome
        	thrustPower =  dna.GetGene(8)*distanceToGoal*Time.deltaTime;
            rotation += dna.GetGene(9) * Time.deltaTime; 
        }
        
        if(!seeLeftGround && !seeRightGround && !seeUpDeath && !seeLeftDeath && !seeRightDeath && !seeGoal)
        { 
            //Retrieving appropriate gene from the agent chromosome
            thrustPower = dna.GetGene(10)*distanceToGoal*Time.deltaTime;
            rotation += dna.GetGene(11) * Time.deltaTime; 
        }

        if(seeGoal)
        {
            //Retrieving appropriate gene from the agent chromosome
            thrustPower =  dna.GetGene(12)*distanceToGoal*Time.deltaTime;
            rotation = dna.GetGene(13) * Time.deltaTime; 
        }

        //Assigning the clamped calculated rotation value
        rotation = Mathf.Clamp(rotation, -maxRot, maxRot);
        var rot = transform.localEulerAngles;
        rot.z = rotation;
        transform.localEulerAngles = rot;

        //Addign the force to the agent accoridng to the clamped calculated thrustPower value
        rb.AddRelativeForce(new Vector3(0,1,0) * Mathf.Clamp(thrustPower, -maxThrustPower, maxThrustPower));  
    }
}
