using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checks to maks sure that a 2D collider is present on the object 
[RequireComponent(typeof(Collider2D))]

public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock { get {return agentFlock; } }

    Collider2D agentCollider;
    //Automatically retrieves the collider and assigns it to the agentCollider object
    public Collider2D AgentCollider { get { return agentCollider; } } 

    GameObject sprite;

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        sprite = this.transform.GetChild(0).gameObject;
    }

    //Assigning the flock to the agent 
    public void Initialize(Flock flock){
        agentFlock = flock;
    }

    //Method which updates the position of the FlockAgent
    public void Move(Vector2 newPos){
        transform.up = newPos;
        transform.position += (Vector3)newPos * Time.deltaTime;

        //Denotes the direction of travel of the flock agents
        Debug.DrawRay(sprite.transform.position, sprite.transform.up, Color.cyan);

        //Denotes how the direction is changing in accordance with the movement direction of the neighbours
        Debug.DrawRay(sprite.transform.position, transform.position.normalized, Color.green);
    }
}
