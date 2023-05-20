using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creating a menu option to simplify Cohesion behaviour creation
[CreateAssetMenu(menuName = "Flock/Behaviour/SteerCohesion")]

//Inherits from FlockBehaviour
public class CohesionBehaviour : FilteredFlockBehaviour
{

    Vector2 currentVelocity;

    //Time taken by agent to get from current state to new state 
    public float agentSmoothTime = 0.5f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock){
        
        /*
        Note: This method is used to force the agents to converge onto the same point

        1) Access the context list which holds a series of transform information and work out its count (context is passed to the CalculateMove method). 
            If said count is 0 return Vector2.zero
            (Hint: Use .Count to get the count)
        2) Create a Vector2 variable named cohesionMove and store in it Vector2.zero  
        3) Create a list of transforms named filteredContext and filter all the transforms in the context list such that only transforms from the same flock are used
            (Hint: If filter is null then the entire context list is used)
            (Hint: Use filter.Filter(agent, context) to filter the context list according to the agent flock)
        4) For each transform in the filteredContext list add its position to the cohesionMove variable and work out its averge. Store the average in cohesionMove.
            (Hint: Use .position to get the objects position)
        5) Pass the cohesionMove variable through the Vector2.SmoothDamp() function and store the result in cohesionMove
            (Hint: Use Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime))
        6) Return cohesionMove
        7) Go into the BehaviourObjects sub directory right click and choose create/Flock/Behaviour/SteerCohesion, this will create a behaviour object.
        8) By clicking on said object you can edit the agent smooth time to smoothen the agent movement   
        9) Select the Composite behaviour and add the newly created behaviour to the compsite one. 
            Specify a weighting for said behaviour, this affects the importance that the behaviour is given in relation to other behaviours. 
            (Hint: A weight of 4 can be set) 
        */

        //No adjustment if no neighbours are close by
        if(context.Count == 0){
            return Vector2.zero;
        }

        //Finding the average position in relation to all the neighbours
        Vector2 cohesionMove = Vector2.zero;

        //If no filter is specified leave context as is otehrwise retrieve teh filtered context (neighbours that abide by the filter)
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

        foreach(Transform item in filteredContext){
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= context.Count;

        //Ensuring that the offset is from the agent position
        cohesionMove -= (Vector2)agent.transform.position;

        //currentVelocity passed as reference so that its value can change
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime);

        return cohesionMove;
    }
}