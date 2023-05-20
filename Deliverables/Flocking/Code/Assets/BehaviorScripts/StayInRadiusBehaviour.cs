using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creating a menu option to simplify Cohesion behaviour creation
[CreateAssetMenu(menuName = "Flock/Behaviour/StayInRadius")]

//Inherits from FlockBehaviour
public class StayInRadiusBehaviour : FlockBehaviour
{
    
    public Vector2 center;
    public float radius = 15f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock){
        /*
        Note: This method is used to keep the agent within a specified area, mainly so that none of the agents go off screen

        1) Calculate the offset of the agent from the center, resulting in a vector pointing in the opposite direction
            (Hint: Use center variable)
        2) Divide the centerOffset magnitude variable created in 1 by the radius of the circle and store it in a variable temp. 
            The value in this variable will denote if the individual agent is within the specified radius or not 
            0 - at the center | < 1 within the radius | > 1 beyond the radius
            (Hint: Use centerOffset.magnitude)
        3) Check if the agent is within the radius, if this is the case return a Vector2.zero otherwise return the centerOffset multiplied by temp squared.
            This force will be used in the composite behavior method which is provided, to apply a force to the agent thus keeping it within the specified area.
        4) Go into the BehaviourObjects sub directory right click and choose create/Flock/Behaviour/StayInRadius, this will create a behaviour object.
        5) By clicking on said object you can edit the circle center and radius, change these values until you are satisifed   
        6) Select the Composite behaviour and add the newly created behaviour to the compsite one. 
            Specify a weighting for said behaviour, this affects the importance that the behaviour is given in relation to other behaviours. 
            (Hint: A weight of 0.1 can be set) 
        */

        //Calculates the offset of the agent from the center, resulting in a vector pointing in the opposite direction 
        Vector2 centerOffset = center - (Vector2)agent.transform.position;

        //0 - at the center | < 1 within the radius | > 1 beyond the radius
        float temp = centerOffset.magnitude / radius;

        if(temp < 0.9f){
            //Change nothing if within the radius 
            return Vector2.zero;
        }
        
        //Pull agent back into the circle
        return centerOffset * temp * temp;
    }
}