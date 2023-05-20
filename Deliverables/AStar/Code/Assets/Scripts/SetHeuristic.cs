using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHeuristic : MonoBehaviour {
    //Declaring variables to be used
    public bool manhattan = true, chebyshev = false, euclidean = false;
    private string tempString = "Manhattan";

    //Display information about heuristic selected and how to change it using GUI
    GUIStyle guiStyle = new GUIStyle();
    void OnGUI() {
        guiStyle.fontSize = 22;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup (new Rect (10, 10, 450, 450));
        GUI.Box (new Rect (10,0,450,450), "Heuristic ", guiStyle);
        GUI.Label(new Rect (10,20,450,450), "Press M for Manhattan Distance", guiStyle);
        GUI.Label(new Rect (10,40,450,450), "Press C for Chebyshev Distance", guiStyle);
        GUI.Label(new Rect (10,60,450,450), "Press E for Euclidean Distance", guiStyle);
        GUI.Label(new Rect (10,80,450,450), "Current Heuristic: " + tempString, guiStyle);
        GUI.EndGroup ();
    }

    void Update() {
        //Call heuristic function depending on keyboard input
        if (Input.GetKeyDown("m")) {
            Manhattan();
        }
        if (Input.GetKeyDown("c")) {
            Chebyshev();
        }
        if (Input.GetKeyDown("e")) {
            Euclidean();
        }
    }

    //Each function sets tempString to its heuristic name, sets its corresponding boolean variable to true and the others to false
    void Manhattan() {
        tempString = "Manhattan";
        manhattan = true;
        chebyshev = false;
        euclidean = false;
    }

    void Chebyshev() {
        tempString = "Chebyshev";
        manhattan = false;
        chebyshev = true;
        euclidean = false;
    }

    void Euclidean() {
        tempString = "Euclidean";
        manhattan = false;
        chebyshev = false;
        euclidean = true;
    }
}
