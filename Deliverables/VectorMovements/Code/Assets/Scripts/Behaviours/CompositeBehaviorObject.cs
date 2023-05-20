using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creating a menu option to simplify Composite behaviour creation
[CreateAssetMenu(menuName = "Behaviours/Composite")]

public class CompositeBehaviorObject : VectorBehavior {

    //Array of VectorBehavior
    public VectorBehavior[] behaviours;
    //Array of weights relating to the behaviour scripts denoting their importance
    public float[] weights;
    public Camera mainCamera;
    //Declare variable for the sum of weights
    private float sumWeights = 0;

    public override Vector3 CalculateMove(Transform current, GameObject target) {
        WrapAroundCameraView(current);
        //Handling unequal array lengths
        if (weights.Length != behaviours.Length){
            Debug.LogError("Data mismatch in " + name, this);
            return Vector3.zero;
        }

        //Calculating movement
        Vector3 move = Vector3.zero;

        //Set sum of weights to zero
        sumWeights = 0;

        //Iterating through the behaviours
        for (int i = 0; i < behaviours.Length; i++) {
            //Set partialMove to the return of the calculate move of the current behaviour multiplied by the current weight
            Vector3 partialMove = behaviours[i].CalculateMove(current, target) * weights[i];
            //If the current weight and partialMove are zero, add the current weight to sum of weights
            if ((weights[i] != 0) && (partialMove != Vector3.zero)) {
                sumWeights += weights[i];
                //Add partialMove to move
                move += partialMove;
            }
        }
        //If sumWeights is not equal to zero, divide move by it
        if (sumWeights != 0) {
            move /= sumWeights;
        }
        //Call the LookAtTarget function and pass current and move
        LookAtTarget(current, move);
        //If move is null, set move to zero vector
        if (move == null) {
            move = Vector3.zero;
        }
      
        return move;
    }

    private void LookAtTarget(Transform current, Vector3 move) {
        //Set direction to the vector difference between location and the game object's position
        Vector3 direction = move - current.position;
        //Normalise direction
        direction.Normalize();
        //Calculate the rotation and set the game object's rotation to the Calculated value
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation -= 90;
        current.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private void WrapAroundCameraView(Transform transform) {
        
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        bool checkScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (!checkScreen)
        {
            if (screenPoint.y > 1) transform.position = new Vector3(transform.position.x, -mainCamera.orthographicSize + 2, transform.position.z);
            else if (screenPoint.y < 0) transform.position = new Vector3(transform.position.x, mainCamera.orthographicSize - 2, transform.position.z);
            else if (transform.position.x > 1) transform.position = new Vector3((-mainCamera.orthographicSize * mainCamera.aspect) + 2, transform.position.y, transform.position.z);
            else if (transform.position.x < 0) transform.position = new Vector3((mainCamera.orthographicSize * mainCamera.aspect) - 2, transform.position.y, transform.position.z);

            checkScreen = false;
        }
    }
}
