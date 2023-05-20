using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
	//Declaring variables for position in the grid
	public int x, y;

	//Declaring variables for current cost, heuristic cost and total cost
	public int gCost, fCost, hCost;

	//Declaring variable to check if a path node can be walked on
	public bool isWalkable;

	//Declaring PathNode to check from which PathNode the game search came from
	public PathNode cameFromNode;

	//Declaring a Cell for the PathNode to be assigned to
	public Cell cell;


	//Constructor
	public PathNode(int x, int y, Cell cell) {
		//Set x, y and corresponding cell to the passed parameters
		this.x = x;
		this.y = y;
		this.cell = cell;
		//Set isWalkable to true
		isWalkable = true;
	}

	//Function to valculate the fcost (add the current cost and heuristic cost together)
	public void CalculateFCost() {
		fCost = gCost + hCost;
	}
}
