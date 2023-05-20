using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGrid {
	//Declaring variables and grid's to be used
	public MainGrid grid;
	public PathNode[,] gridArray;
	private Vector3 originPosition;
	private float cellSize;
	private int width, height;

	//Constructor
	public PathGrid(int width, int height, float cellSize, Vector3 originPosition) {
		//Find MainGrid of GameObject GameManager and save it in grid
		grid = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MainGrid>();
		//Set width, height, cellSize and originPosition to the passed parameters
		this.width = width;
		this.height = height;
		this.cellSize = cellSize;
		this.originPosition = originPosition;
		//Set gridArray to a 2D PathNode array of size grid.sizeOfGrid
		gridArray = new PathNode[grid.sizeOfGrid.x, grid.sizeOfGrid.y];

		//For each element in gridArray, create a new PathNode with parameters x, y, and the cell at that respective position
		for (int x = 0; x < gridArray.GetLength(0); x++) {
			for (int y = 0; y < gridArray.GetLength(1); y++) {
				gridArray[x, y] = new PathNode(x, y, grid.GetCellAtGridIndexPos(new Vector2Int(x, y), new Vector2Int(0, 0)));
			}
		}

	}
}
