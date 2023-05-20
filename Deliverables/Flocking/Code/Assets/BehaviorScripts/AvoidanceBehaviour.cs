using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creating a menu option to simplify Avoidance behaviour creation
[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]

//Inherits from FlockBehaviour
public class AvoidanceBehaviour : FilteredFlockBehaviour

{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock){
        
        /*
        Note: This method is used to indicate to the agents what they should avoid and how to avoid it

        1) Access the context list which holds a series of transform information and work out its count (context is passed to the CalculateMove method). 
            If said count is 0 return Vector2.zero
            (Hint: Use .Count to get the count)
        2) Create a Vector2 variable named avoidanceMove and store in it Vector2.zero  
        3) Create an integer variable named numAvoid and set it to 0
        4) Create a list of transforms named filteredContext and filter all the transforms in the context list such that only transforms from the same flock are used
            (Hint: If filter is null then the entire context list is used)
            (Hint: Use filter.Filter(agent, context) to filter the context list according to the agent flock)
        5) For each transform in the filteredContext list check if the square distance between the agent and its neighbour is less than the specified radius.
            If this is the case increment numAvoid and work out the difference between the agent and the object to avoid. 
            Add this value to avoidanceMove and then work out its avergae, store the result in avoidanceMove.
            (Hint: Use .position to get the objects position)
            (Hint: Use numAvoid to work out the average)
        6) Return avoidanceMove
        7) Go into the BehaviourObjects sub directory right click and choose create/Flock/Behaviour/Avoidance, this will create a behaviour object.
        8) By clicking on said object you can edit the filter used. In this case enter the FilterObject directory and drag the SameFlockFilter into the flock area.   
        9) Select the Composite behaviour and add the newly created behaviour to the compsite one. 
            Specify a weighting for said behaviour, this affects the importance that the behaviour is given in relation to other behaviours. 
            (Hint: A weight of 10 can be set) 
        */

        //No adjustment if no neighbours are close by
        if(context.Count == 0){
            return Vector2.zero;
        }

        //Finding the average position in relation to all the neighbours
        Vector2 avoidanceMove = Vector2.zero;

        //Number of neighbours to avoid 
        int numAvoid = 0;

        //If no filter is specified leave context as is otehrwise retrieve teh filtered context (neighbours that abide by the filter)
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

        foreach(Transform item in filteredContext){
            //Checking if the square distance between agent and neighbour is less than the specified radius
            if(Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius){
                numAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position);
            }
        }

        //Finding the average
        if (numAvoid > 0){
            avoidanceMove /= numAvoid;
        }

        return avoidanceMove;
    }
}
