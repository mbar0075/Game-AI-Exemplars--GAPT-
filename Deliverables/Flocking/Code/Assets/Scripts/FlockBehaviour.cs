using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehaviour : ScriptableObject
{
    //flockAgent to calculate its moves
    //context list of Transforms of neighbouring agents
    //flock is a Flock object to retrieve required information 
    public abstract Vector2 CalculateMove (FlockAgent agent, List<Transform> context, Flock flock); 
}
