using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{

    public float shieldArea = 5.0f;

    //FlockAgent prefab
    public FlockAgent agentPrefab;
    //List of FlockAgent
    List<FlockAgent> agents = new List<FlockAgent>();
    //Script of FlockBehaviour type
    public FlockBehaviour behaviour;

    //Denotes the amount of agents created on startup can range from 10 to 500, default value 250
    [Range(10, 500)]
    public int startingAmount = 250;
    //Value used to denote the starting area of the Flock 
    const float AgentDensity = 0.08f;

    //Multiplies speed of agent to make it move faster to mitigate counter acting movements  
    [Range(1f, 100f)]
    public float driveFactor = 10f;
    //Max Speed of the agent
    [Range(1f, 100f)]
    public float maxSpeed = 5f;

    //Used to denote the radius in which neighbours are considered  
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    //Used to denote the radius in which neighbours are avoided  
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    //Values used to reduce repeated calculation execution 
    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squareAvoidanceRadius;
    //Returns the squareAvoidanceRadius when called
    public float SquareAvoidanceRadius {get {return squareAvoidanceRadius;}}


    //Values used in relation to the Move To Place behaviour
    public bool MoveToPlaceActive = false;
    public float MoveToPlaceCircleRadius = 15f;
    public float timer = 60f;
    Vector2 position = Vector2.zero;
    
    public bool showCircleGizmos = true;

    // Start is called before the first frame update
    void Start()
    {
        //Calculating the required square values
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingAmount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab, 
                Random.insideUnitCircle * startingAmount * AgentDensity, //The spawn area for the agents is within a circle whose radius is based on the startingAmount & AgentDensity
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), //The agent can spawn facing any direction
                transform //Setting the parent of the Agent
            );
            
            //Assigning a name to each agent
            newAgent.name = "Agent " + i;

            //Assigning the Flock to the agent
            newAgent.Initialize(this);

            //Adding the newAgent to the list of agents
            agents.Add(newAgent);

        }

        //Runs the method only if its selected as active
        if(MoveToPlaceActive == true){
            InvokeRepeating("MoveToPlace",1.0f, timer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Updating the values in for testing purposes
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        foreach(FlockAgent agent in agents){
            //Retrieving the Transforms of all nearby agents
            List<Transform> context = GetNearbyObjects(agent);

            //Passing the agent, list of neighbour agent transforms and the Flock and 
            //calling the CalculateMove method from the behaviour script
            //Vector2 move = behaviour.CalculateMove(agent, context, this);
            Vector2 move = behaviour.CalculateMove(agent, context, this);

            //Speeds up movement
            move *= driveFactor;
            
            if(MoveToPlaceActive == true){
                move += position - (Vector2)agent.transform.position;
            }

            //Clamping the move to the maxSpeed
            if(move.sqrMagnitude > squareMaxSpeed){
                move = move.normalized * maxSpeed;
            } 

            //Moving the agent as required
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent){

        List<Transform> context = new List<Transform>();
        
        //Casting a circle of radius neighbourRadius with the agent as the center and retrieving all the collider2D that intersect with said circle
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);

        foreach(Collider2D c in contextColliders){
            //If collider isn't the agent collider of the current agent
            if(c != agent.AgentCollider){
                
                //Adding the transform of the agent to the list 
                context.Add(c.transform);
            }
        }

        return context;
    }

    //Changes the value in the position for the boids to travel to
    void MoveToPlace(){
        position = Random.insideUnitCircle * MoveToPlaceCircleRadius; 
    }

    //Method to draw the required gizmos
    void OnDrawGizmos()
    {    
        if(showCircleGizmos){
            foreach (var agent in agents)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(agent.transform.position, squareNeighbourRadius);

                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(agent.transform.position, squareAvoidanceRadius);
            }
        }
    }

    GUIStyle guiStyle = new GUIStyle();
	void OnGUI()
	{
		guiStyle.fontSize = 10;
		guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup (new Rect (10, 10, 250, 150));
		GUI.Box (new Rect (0,0,140,140), "Legend:", guiStyle);
		GUI.Label(new Rect (0,25,200,10), "Gray: Neighbour Detection Area", guiStyle);
        GUI.Label(new Rect (0,50,200,30), "Pink: Avoidance Detection Area", guiStyle);
		GUI.Label(new Rect (0,75,200,30), "Cyan: Looking Direction", guiStyle);

        //Shows how the agent changes direction from old to new
		GUI.Label(new Rect (0,100,200,30), "Green: Force Change Visualiser", guiStyle);
		GUI.EndGroup ();
	}
}
