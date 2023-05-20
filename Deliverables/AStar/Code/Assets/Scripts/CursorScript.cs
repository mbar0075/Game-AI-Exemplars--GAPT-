using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CursorScript : MonoBehaviour {
    //Camera assigned by the user
    [SerializeField] private Camera mainCamera;

    //On start, set the main camera and hide the cursor
    private void Start() {
        mainCamera = Camera.main;
    }

    private void Update() {
        //Set the position of the game object to the mouse's position
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
    }
}