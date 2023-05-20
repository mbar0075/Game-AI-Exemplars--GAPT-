using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlaceTower : MonoBehaviour {
    //Declaring game objects and variables to be used
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField] private GameObject weapon = null;
    private Vector3Int previousMousePos = new Vector3Int();

    //Function to get the cell the mouse is currently hovering on's Vector3 position
    public Vector3Int GetMousePosition() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    void Start() {
        //Get the grid game object
        grid = gameObject.GetComponent<Grid>();
    }

    void Update() {
        //Store the current mouse position in mousePos
        Vector3Int mousePos = GetMousePosition();
        //If the mousePos is no longer equal to previousMousePos
        if (!mousePos.Equals(previousMousePos)) {
            //Create a circle collider and check with any collisions from layer 9, store them in intersecting
            Collider2D intersecting = Physics2D.OverlapCircle(mousePos + new Vector3(0.5f, 0.5f, 0), 0.3f, 1 << 9);
            //If the tile exists and intersecting is null, make it green
            if ((interactiveMap.GetTile(mousePos + new Vector3Int(-15, -5, 0))) && (intersecting == null)) {
                interactiveMap.SetTileFlags(mousePos + new Vector3Int(-15, -5, 0), TileFlags.None);
                interactiveMap.SetColor(mousePos + new Vector3Int(-15, -5, 0), Color.green);
            //Otherwise make it red
            } else {
                interactiveMap.SetTileFlags(mousePos + new Vector3Int(-15, -5, 0), TileFlags.None);
                interactiveMap.SetColor(mousePos + new Vector3Int(-15, -5, 0), Color.red);
            }  
            //Set the previousMousePos tile to its original color
            interactiveMap.SetTileFlags(previousMousePos + new Vector3Int(-15, -5, 0), TileFlags.None);
            interactiveMap.SetColor(previousMousePos + new Vector3Int(-15, -5, 0), new Color(255, 255, 255));
            //Set previousMousePos to mousePos
            previousMousePos = mousePos;
        }

        //On left click
        if ((Input.GetMouseButtonDown(0))) {  
            //Create a circle collider and check with any collisions from layer 9, store them in intersecting
            Collider2D intersecting = Physics2D.OverlapCircle(mousePos + new Vector3(0.5f, 0.5f, 0), 0.3f, 1 << 9);
            //If the tile exists and intersecting is null, instantiate a weapon at the position and change the tile's color to red
            if ((interactiveMap.GetTile(mousePos + new Vector3Int(-15, -5, 0))) && (intersecting == null)) {
                Instantiate(weapon, mousePos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                interactiveMap.SetTileFlags(mousePos + new Vector3Int(-15, -5, 0), TileFlags.None);
                interactiveMap.SetColor(mousePos + new Vector3Int(-15, -5, 0), Color.red);
            }
        }
    }
}