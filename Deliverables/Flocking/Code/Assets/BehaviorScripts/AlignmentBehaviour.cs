using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creating a menu option to simplify Alignment behaviour creation
[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]

//Inherits from FlockBehaviour
public class AlignmentBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock){
        
        /*
        Note: This method is used to align the agents to one another so that they face the same direction whilst moving 

        1) Check if the context count is 0, if this is the case return the agent.transform.up . 
            This makes sure that the agent keeps facing whichever direction it was facing.
        2) Create a Vector2 variable named alignmentMove and store in it Vector2.zero  
        3) Create a list of transforms named filteredContext and filter all the transforms in the context list such that only transforms from the same flock are used
            (Hint: If filter is null then the entire context list is used)
            (Hint: Use filter.Filter(agent, context) to filter the context list according to the agent flock)
        4) For each transform in the filteredContext list add the transform.up of each transform to the alignmentMove and work out its average. Store the result in alignmentMove.
        5) Return alignmentMove
        6) Go into the BehaviourObjects sub directory right click and choose create/Flock/Behaviour/Alignment, this will create a behaviour object.
        7) By clicking on said object you can edit the filter used. In this case enter the FilterObject directory and drag the SameFlockFilter into the flock area.   
        8) Select the Composite behaviour and add the newly created behaviour to the compsite one. 
            Specify a weighting for said behaviour, this affects the importance that the behaviour is given in relation to other behaviours. 
            (Hint: A weight of 1 can be set) 
        */

        //Retain current position if no neighbours are close by
        if(context.Count == 0){
            return agent.transform.up;
        }

        //Finding the average position in which to face in relation to all the neighbours
        Vector2 alignmentMove = Vector2.zero;
        
        //If no filter is specified leave context as is otehrwise retrieve teh filtered context (neighbours that abide by the filter)
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);

        foreach(Transform item in filteredContext){
            alignmentMove += (Vector2)item.transform.up;
        }
        //Return a normzlized value as all transform.up's are normalized
        alignmentMove /= context.Count;
        return alignmentMove;
    }
}
