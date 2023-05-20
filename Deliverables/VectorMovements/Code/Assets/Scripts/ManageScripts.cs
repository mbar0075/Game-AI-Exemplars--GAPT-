using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageScripts : MonoBehaviour {
    //Declaring Variables
    [SerializeField] private GameObject target;
    [SerializeField] private bool showArriveCircle = false, showFollowTLCircle = false, showTargetLine = false, showCurrentDirection = false;
    [SerializeField] private float arriveRadius = 5f, FollowTheLeaderRadius = 5f, directionLineLength = 5f;
    //Script of VectorBehavior type
    public VectorBehavior behaviour;

    GUIStyle guiStyle = new GUIStyle();
    void OnGUI() {
        int tempNum = 10;
        guiStyle.fontSize = 16;
        guiStyle.normal.textColor = Color.green;
        GUI.BeginGroup (new Rect (10, 10, 2000, 2000));
        if (showArriveCircle || showFollowTLCircle) {
            GUI.Box (new Rect (10,tempNum,450,450), "Green Circle - Arrival Radius", guiStyle);
            tempNum += 20;
        }
        guiStyle.normal.textColor = Color.yellow;
        if (showFollowTLCircle) {
            GUI.Box (new Rect (10,tempNum,450,450), "Yellow Circle - Evade Radius", guiStyle);
            tempNum += 20;
        }
        guiStyle.normal.textColor = Color.red;
        if (showTargetLine) {
            GUI.Box (new Rect (10,tempNum,450,450), "Red Line - Line to Target", guiStyle);
            tempNum += 20;
        }
        guiStyle.normal.textColor = Color.blue;
        if (showCurrentDirection) {
            GUI.Box (new Rect (10,tempNum,450,450), "Blue Line - Current Direction", guiStyle);
            tempNum += 20;
        }
        guiStyle.normal.textColor = Color.white;
        GUI.Box (new Rect (1200,10,450,450), "White Line - Wander Direction", guiStyle);
        guiStyle.normal.textColor = Color.magenta;
        GUI.Box (new Rect (1200,30,450,450), "Magenta Line - Pursuit / Evade Prediction", guiStyle);

        //GUI.Label(new Rect (10,20,450,450), "Press M for Manhattan Distance", guiStyle);;
        GUI.EndGroup ();
    }

    void Update() {
        //Call the calculate move function on behaviour and pass the current transform as well as the target game object
        Vector3 move = behaviour.CalculateMove(this.transform, target);
        //Set the position of the game object to the returned Vector3 of the function
        transform.position = move;
    }

    void OnDrawGizmos() {
        //If button is selected, show the circle for Arrive
        if (showArriveCircle) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(target.transform.position, arriveRadius);
        }
        //If button is selected, show the circles for Follow the Leader
        if (showFollowTLCircle) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(target.transform.position, FollowTheLeaderRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(target.transform.position, FollowTheLeaderRadius * 0.75f);
        }
        //If button is selected, show line to target
        if (showTargetLine) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
        //If button is selected, show current directions facing ray
        if (showCurrentDirection) {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position,  transform.up * directionLineLength);
        }
    }
}
