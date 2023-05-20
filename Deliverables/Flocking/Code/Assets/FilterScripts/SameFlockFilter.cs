using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creating a menu option to simplify SameFlock behaviour creation
[CreateAssetMenu(menuName = "Flock/Filter/SameFlock")]

public class SameFlockFilter : ContextFilter
{
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original){

        //List of transforms that are going to be considered
        List<Transform> filtered = new List<Transform>();
        foreach (Transform item in original){
            FlockAgent itemAgent = item.GetComponent<FlockAgent>();

            //If agent belongs to the same Flock add it to the list
            if(itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock){
                filtered.Add(item);
            }
        }
        return filtered;
    }
}
