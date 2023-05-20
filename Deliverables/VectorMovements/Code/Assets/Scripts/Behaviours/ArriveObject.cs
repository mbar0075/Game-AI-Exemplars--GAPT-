using UnityEngine;

//Creating a menu option to simplify Arrive behaviour creation
[CreateAssetMenu(menuName = "Behaviours/Arrive")]

public class ArriveObject : VectorBehavior {

    //Declaring Max speed and force (steering force) and radius
    [SerializeField] protected float maxSpeed, maxForce, maxRadius;
    //Initialising variables to be used to Vector3 zero
    private Vector3 acceleration, velocity, location, startPosition = Vector3.zero; 

    public override Vector3 CalculateMove(Transform current, GameObject target) {
        //Set location to the game object's position
        location = current.position;
        //Set startPosition to the game object's position
        startPosition = current.position;

        //Call the steering function and pass the target game object and the current transform
        SteeringFunction(current, target);
        //Call the apply steering function and save the returned vector in tempLocation
        Vector3 tempLocation = ApplySteering();
        return tempLocation;
    }

    private void SteeringFunction(Transform current, GameObject target) {
        //Set changeVelocity to the difference in vector positions between the target and the game object
        Vector3 changeVelocity = target.transform.position - current.position;
        //Normalise changeVelocity
        changeVelocity.Normalize();
        //Set distance to the distance between the location variable and the game object
        float distance = Vector3.Distance(target.transform.position, location);
        //If the distance is smaller than the maximum radius, multiply changeVelocity by distance, otherwise multiply it by the maxSpeed 
        if (distance < maxRadius) {
            changeVelocity *= distance;
        } else { 
            changeVelocity *= maxSpeed;
        }
        //Set steer to difference between changeVelocity and velocity, with a magnitude clamped at maxForce
        Vector3 steer = Vector3.ClampMagnitude(changeVelocity - velocity, maxForce);
        //Add steer to acceleration
        acceleration += steer;
    }

    private Vector3 ApplySteering() {
        //Set velocity to the addition of velocity and acceleration, with a magnitude clamped at maxSpeed
        velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);
        //Add velocity to location
        location += velocity * Time.deltaTime;
        //Set acceleration back to zero
        acceleration = Vector3.zero;
        return location;
    }
}