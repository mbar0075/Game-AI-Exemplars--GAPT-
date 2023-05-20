using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creating a menu option to simplify Cohesion behaviour creation
[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]

public class CompositeBehaviour : FlockBehaviour
{

    //Array of FlockBehaviour
    public FlockBehaviour[] behaviours;
    //Array of weights relating to the behaviour scripts denoting their importance
    public float[] weights;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock){
        
        //Handling unequal array lengths
        if (weights.Length != behaviours.Length){
            Debug.LogError("Data mismatch in " + name, this);
            return Vector2.zero;
        }

        //Calculating movemnet
        Vector2 move = Vector2.zero;

        //Iterating through the behaviours
        for (int i = 0; i < behaviours.Length; i++)
        {
            Vector2 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i];

            //If some movemnt is being done clamp movement
            if(partialMove != Vector2.zero){
                if(partialMove.sqrMagnitude > weights[i] * weights[i]){
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }
        
        return move;
    }
}
