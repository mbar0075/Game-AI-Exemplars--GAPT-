using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VectorBehavior : ScriptableObject {
    //Abstract function to Calculate the move of a behaviour and returns Vector3
    public abstract Vector3 CalculateMove (Transform current, GameObject target); 
}
