using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MainGrid : MonoBehaviour {
    //Variable which holds a 2d array of Cells (Grid)
    [SerializeField] public Cell [,] cellGrid;
    //Variable which holds the size of the Grid
    [SerializeField] public Vector2Int sizeOfGrid;
    //Variable which holds hte Cell Radius
    [SerializeField] public float cellRadius;
    //Boolean variable which will determine whether to draw Grid or not
    [SerializeField] private bool drawGrid;
    //Boolean variable which will determine whether to draw Cost Field or not
    [SerializeField] private bool drawCostField;
    //Boolean variable which will determine whether to draw Integration Field or not
    [SerializeField] private bool drawIntegrationField;
    //Boolean variable which will determine whether to draw Flow Field or not
    [SerializeField] private bool drawFlowField;
    //LayerMask variable to hold the Obstacle Layer
    [SerializeField] private LayerMask obstacleLayer;
    //Cell variable, which will hold the destination
    public Cell destinationCell;
    //Boolean variable which will determine whether mouse is clicked or not
    public bool mouseclicked=false;
    //Boolean variable which will determine whether to draw primary jump points or not
    [SerializeField] private bool primaryJumpPoints = false;
    //Boolean variable which will determine whether to draw other jump points or not
    [SerializeField] private bool otherJumpPoints = false;



    //Constructor for Class
    public MainGrid(float cRadius, Vector2Int gridSize){
        //Set cellRadius and sizeOfGrid to the passed parameters
        cellRadius=cRadius;
        sizeOfGrid=gridSize;
    }

    //1) Method which creates the Field Grid
    private void CreateFieldGrid(){
        //Initialising cellGrid array
        cellGrid= new Cell[sizeOfGrid.x,sizeOfGrid.y];
        //Populating the Grid with Cells
        for(int i=0; i<sizeOfGrid.x;i++){
            for(int j=0; j<sizeOfGrid.y;j++){
                //Creating Vector3 position for grid utilising calculated offset and current position
                Vector3 worldPosition = new Vector3(cellRadius*2*i+cellRadius+transform.position.x,cellRadius*2*j+cellRadius+transform.position.y,transform.position.z);
                //Calling Cell Constructor
                cellGrid[i,j]=new Cell(worldPosition, new Vector2Int(i,j));
            }
        }
    }

    //2) Method which creates the Cost Field
    private void CreateCostField(){
        //Variable to hold the half extents
        Vector3 halfExtents = Vector2.one*cellRadius;
        //Looping through all the Cells in the cellGrid
        foreach (Cell currentCell in cellGrid){
            //Checking whether there were any collisions, with the obstacle layer, by Physics2D.OeverlapBox method
            //This method, creates an invisible box around the game object, acting as a collider, note that current object does not need to have a collider
            //In this case Physics2D is used since we are working with tilemaps
            bool obstacles = Physics2D.OverlapBox(currentCell.worldPosition,halfExtents,0,obstacleLayer.value);
            //Checking whether there was a collision with a obstacle layer game object and if so, proceeding to set the currentCell's cost to the max cost
            if(obstacles!=false){
                currentCell.AddToCost(255);
                currentCell.isWall = true;
                    continue;
            }
        }
    }

    private void JumpPoints() {
        //Loop for the four types of jump points
        //isPrimaryJP is a boolean variable used to check if a cell is a primary jump point
        //rightForce, leftForce, upForce and downForce are used to check the direction of travel of a primary jump point
        //isWall is used to check if the cell is a wall or not
        //jpType determines the type of stage of jump points to be generated
        //straightDown, straightLeft, straightRight, straightUp, diaDL, diaDR, diaUL and diaUR determine the distances between the current cell and either another jump point or a wall
        for (int jpType = 1; jpType <= 4; jpType++) {
            for(int i=0; i<sizeOfGrid.x;i++){
                for(int j=0; j<sizeOfGrid.y;j++){
                    //Primary Jump Points
                    //Jump points are created if forced neighbours exist
                    if (jpType == 1) {
                        if (!((i == 0) || (i == sizeOfGrid.x-1) || (j == 0) || (j == sizeOfGrid.y-1))) {
                            if (((cellGrid[i,j+1]).isWall) && (!(cellGrid[i,j]).isWall)) {
                                if ((!(cellGrid[i+1,j+1]).isWall)) {
                                    if (!((cellGrid[i+1,j]).isWall)) {
                                        cellGrid[i+1,j].isPrimaryJP = true;
                                        cellGrid[i+1,j].rightForce = true;
                                    }
                                }
                                if ((!(cellGrid[i-1,j+1]).isWall)) {
                                    if (!((cellGrid[i-1,j]).isWall)) {
                                        cellGrid[i-1,j].isPrimaryJP = true;
                                        cellGrid[i-1,j].leftForce = true;
                                    }
                                }
                            }
                            if (((cellGrid[i,j-1]).isWall) && (!(cellGrid[i,j]).isWall)) {
                                if ((!(cellGrid[i+1,j-1]).isWall)) {
                                    if (!((cellGrid[i+1,j]).isWall)) {
                                        cellGrid[i+1,j].isPrimaryJP = true;
                                        cellGrid[i+1,j].rightForce = true;
                                    }
                                }
                                if ((!(cellGrid[i-1,j-1]).isWall)) {
                                    if (!((cellGrid[i-1,j]).isWall)) {
                                        cellGrid[i-1,j].isPrimaryJP = true;
                                        cellGrid[i-1,j].leftForce = true;
                                    }
                                }
                            }
                            if (((cellGrid[i+1,j]).isWall) && (!(cellGrid[i,j]).isWall)) {
                                if ((!(cellGrid[i+1,j+1]).isWall)) {
                                    if (!((cellGrid[i,j+1]).isWall)) {
                                        cellGrid[i,j+1].isPrimaryJP = true;
                                        cellGrid[i,j+1].upForce = true;
                                    }
                                }
                                if ((!(cellGrid[i+1,j-1]).isWall)) {
                                    if (!((cellGrid[i,j-1]).isWall)) {
                                        cellGrid[i,j-1].isPrimaryJP = true;
                                        cellGrid[i,j-1].downForce = true;
                                    }
                                }
                            }
                            if (((cellGrid[i-1,j]).isWall) && (!(cellGrid[i,j]).isWall)) {
                                if ((!(cellGrid[i-1,j+1]).isWall)) {
                                    if (!((cellGrid[i,j+1]).isWall)) {
                                        cellGrid[i,j+1].isPrimaryJP = true;
                                        cellGrid[i,j+1].upForce = true;
                                    }
                                }
                                if ((!(cellGrid[i-1,j-1]).isWall)) {
                                    if (!((cellGrid[i,j-1]).isWall)) {
                                        cellGrid[i,j-1].isPrimaryJP = true;
                                        cellGrid[i,j-1].downForce = true;
                                    }
                                }
                            }              
                        }
                    //Straight Jump Points
                    //Assign cardinal values to the numbers of cells needed to run into a primary jump point for that direction of travel 
                    } else if (jpType == 2) {
                        if (!((i == 0) || (i == sizeOfGrid.x-1) || (j == 0) || (j == sizeOfGrid.y-1))) {
                            for(int k=i+1; k<sizeOfGrid.x-1;k++) {
                                if ((cellGrid[k, j].isWall)) {
                                    break;
                                }
                                if ((cellGrid[k,j].isPrimaryJP) && (cellGrid[k,j].rightForce)) {
                                    cellGrid[i,j].straightRight = Math.Abs(i-k);
                                    break;
                                }
                            }
                            for(int k=i-1; k>0;k--) {
                                if ((cellGrid[k, j].isWall)) {
                                    break;
                                }
                                if ((cellGrid[k,j].isPrimaryJP) && (cellGrid[k,j].leftForce)) {
                                    cellGrid[i,j].straightLeft = Math.Abs(i-k);
                                    break;
                                }
                            }
                            for(int l=j+1; l<sizeOfGrid.y-1;l++) {
                                if ((cellGrid[i, l].isWall)) {
                                    break;
                                }
                                if ((cellGrid[i,l].isPrimaryJP) && (cellGrid[i,l].upForce)) {
                                    cellGrid[i,j].straightUp = Math.Abs(j-l);
                                    break;
                                }
                            }
                            for(int l=j-1; l>0;l--) {
                                if ((cellGrid[i, l].isWall)) {
                                    break;
                                }
                                if ((cellGrid[i,l].isPrimaryJP) && (cellGrid[i,l].downForce)) {
                                    cellGrid[i,j].straightDown = Math.Abs(j-l);
                                    break;
                                }
                            }
                        }
                    //Diagonal Jump Points
                    //Set the diagonal values to the shortest distance between the current cell and a cell in which a diagonal direction of travel will reach either a primary jump point
                    //or a straight jump point that is traveling in its cardinal directions (Ex: If travelling to top right, cardinal directions are up and right)
                    } else if (jpType == 3) {
                        if (!((i == 0) || (i == sizeOfGrid.x-1) || (j == 0) || (j == sizeOfGrid.y-1))) {
                            int k = 1;
                            while ((!((i+k == 0) || (i+k == sizeOfGrid.x-1))) && (!((j+k == 0) || (j+k == sizeOfGrid.y-1))) && (!(cellGrid[i+k, j+k].isWall))) {
                                if (!(cellGrid[i+k, j+k].isWall)) {
                                    if (cellGrid[i+k,j+k].straightUp > 0) { 
                                        cellGrid[i,j].diaUR = k;
                                        break;
                                    }
                                    if (cellGrid[i+k,j+k].straightRight > 0) { 
                                        cellGrid[i,j].diaUR = k;
                                        break;
                                    }
                                }
                                k++;
                            }
                            k = 1;
                            while ((!((i-k == 0) || (i-k == sizeOfGrid.x-1))) && (!((j-k == 0) || (j-k == sizeOfGrid.y-1))) && (!(cellGrid[i-k, j-k].isWall))) {
                                if (!(cellGrid[i-k, j-k].isWall)) {
                                    if (cellGrid[i-k,j-k].straightDown > 0) { 
                                        cellGrid[i,j].diaDL = k;
                                        break;
                                    }
                                    if (cellGrid[i-k,j-k].straightLeft > 0) { 
                                        cellGrid[i,j].diaDL = k;
                                        break;
                                    }  
                                }
                                k++;
                            }
                            k = 1;
                            while ((!((i-k == 0) || (i-k == sizeOfGrid.x-1))) && (!((j+k == 0) || (j+k == sizeOfGrid.y-1))) && (!(cellGrid[i-k, j+k].isWall))) {
                                if (!(cellGrid[i-k, j+k].isWall)) {
                                    if (cellGrid[i-k,j+k].straightUp > 0) { 
                                        cellGrid[i,j].diaUL = k;
                                        break;
                                    }
                                    if (cellGrid[i-k,j+k].straightLeft > 0) { 
                                        cellGrid[i,j].diaUL = k;
                                        break;
                                    }  
                                }
                                k++;
                            }
                            k = 1;
                            while ((!((i+k == 0) || (i+k == sizeOfGrid.x-1))) && (!((j-k == 0) || (j-k == sizeOfGrid.y-1))) && (!(cellGrid[i+k, j-k].isWall))) {
                                if (!(cellGrid[i+k, j-k].isWall)) {
                                    if (cellGrid[i+k,j-k].straightDown > 0) { 
                                        cellGrid[i,j].diaDR = k;
                                        break;
                                    }
                                    if (cellGrid[i+k,j-k].straightRight > 0) { 
                                        cellGrid[i,j].diaDR = k;
                                        break;
                                    }  
                                }
                                k++;
                            }
                        }
                    //Wall Distances
                    //If a direction has a value of zero set it to the negative distance between it and the nearest wall of that direction
                    } else if (jpType == 4) {
                        if (!((i == 0) || (i == sizeOfGrid.x-1) || (j == 0) || (j == sizeOfGrid.y-1))) {
                            if (!(cellGrid[i, j].isWall)) {
                                if (cellGrid[i, j].straightLeft == 0) {
                                    for(int k=i; k>=0;k--) {
                                        if ((k == 0) || (cellGrid[k, j].isWall)) {
                                            cellGrid[i,j].straightLeft = (-(Math.Abs(i-k)) + 1);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!(cellGrid[i, j].isWall)) {
                                if (cellGrid[i, j].straightRight == 0) {
                                    for(int k=i; k<=sizeOfGrid.x-1;k++) {
                                        if ((k == sizeOfGrid.x-1) || (cellGrid[k, j].isWall)) {
                                            cellGrid[i,j].straightRight = (-(Math.Abs(i-k)) + 1);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!(cellGrid[i, j].isWall)) {
                                if (cellGrid[i, j].straightUp == 0) {
                                    for(int k=j; k<=sizeOfGrid.y-1;k++) {
                                        if ((k == sizeOfGrid.y-1) || (cellGrid[i, k].isWall)) {
                                            cellGrid[i,j].straightUp = (-(Math.Abs(j-k)) + 1);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!(cellGrid[i, j].isWall)) {
                                if (cellGrid[i, j].straightDown == 0) {
                                    for(int k=j; k>=0;k--) {
                                        if ((k == 0) || (cellGrid[i, k].isWall)) {
                                            cellGrid[i,j].straightDown = (-(Math.Abs(j-k)) + 1);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!(cellGrid[i, j].isWall)) {
                                if (cellGrid[i, j].diaUR == 0) {
                                    int k = 1;
                                    while (true) {
                                        if ((((i+k == 0) || (i+k == sizeOfGrid.x-1))) || (((j+k == 0) || (j+k == sizeOfGrid.y-1))) || ((cellGrid[i + k, j + k].isWall))) {
                                            cellGrid[i,j].diaUR = (-k) + 1;
                                            break;
                                        }
                                        k++;
                                    }
                                }
                            }
                            if (!(cellGrid[i, j].isWall)) {
                                if (cellGrid[i, j].diaUL == 0) {
                                    int k = 1;
                                    while (true) {
                                        if ((((i-k == 0) || (i-k == sizeOfGrid.x-1))) || (((j+k == 0) || (j+k == sizeOfGrid.y-1))) || ((cellGrid[i - k, j + k].isWall))) {
                                            cellGrid[i,j].diaUL = (-k) + 1;
                                            break;
                                        }
                                        k++;
                                    }
                                }
                            }
                            if (!(cellGrid[i, j].isWall)) {
                                if (cellGrid[i, j].diaDL == 0) {
                                    int k = 1;
                                    while (true) {
                                        if ((((i-k == 0) || (i-k == sizeOfGrid.x-1))) || (((j-k == 0) || (j-k == sizeOfGrid.y-1))) || ((cellGrid[i - k, j - k].isWall))) {
                                            cellGrid[i,j].diaDL = (-k) + 1;
                                            break;
                                        }
                                        k++;
                                    }
                                }
                            }
                            if (!(cellGrid[i, j].isWall)) {
                                if (cellGrid[i, j].diaDR == 0) {
                                    int k = 1;
                                    while (true) {
                                        if ((((i+k == 0) || (i+k == sizeOfGrid.x-1))) || (((j-k == 0) || (j-k == sizeOfGrid.y-1))) || ((cellGrid[i + k, j - k].isWall))) {
                                            cellGrid[i,j].diaDR = (-k) + 1;
                                            break;
                                        }
                                        k++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //3) Method which creates the Flow Field
    private void CreateFlowField()
    {
        //Looping through all the Cells in the grid
        foreach(Cell currentCell in cellGrid)
        {
            //Retrieving the Cell Neighbors, and storing them in a list of Cells
            List<Cell> cellNeighbors = GetNeighboringCells(currentCell.gridIndex, Vectors.allDirections);
            //Storing the current Cell's best Cost
            int bestCost = currentCell.bestCost;
            //Looping through all the neighbors
            foreach(Cell neighbor in cellNeighbors)
            {
                //if neighbor's best cost is better, then take the neighbors' cost
                if(neighbor.bestCost < bestCost) {
                    bestCost = neighbor.bestCost;
                    Vector2Int vector=neighbor.gridIndex - currentCell.gridIndex;
                    currentCell.bestDirection = new Vectors(vector.x,vector.y);
                }
            }
        }
    }

    //On Draw Gizmos method, used for debugging purposes, to show grid, and fields in Scene View
    private void OnDrawGizmos(){
        //Checking whether drwGrid is true
        if(drawGrid){
            //Setting Gizmos color to green
            Gizmos.color= Color.green;
            //Looping through all the Cells in the Grid
            for(int i=0; i<sizeOfGrid.x;i++){
                for(int j=0; j<sizeOfGrid.y;j++){
                    if (!(cellGrid[i, j].isWall)) {
                        //Drawing cubes via Vector3 for grid utilising calculated offset and current position
                        Vector3 center = new Vector3(cellRadius*2*i+cellRadius+transform.position.x,cellRadius*2*j+cellRadius+transform.position.y,transform.position.z);
                        Vector3 size = Vector3.one*cellRadius*2;
                        Gizmos.DrawWireCube(center,size);
                        //Display jump point values if enabled
                        if (otherJumpPoints) {
                            Handles.Label(center + new Vector3(0f, -cellRadius/1.5f, 0f),cellGrid[i,j].straightDown.ToString());
                            Handles.Label(center + new Vector3(0f, cellRadius/1.5f, 0f),cellGrid[i,j].straightUp.ToString());
                            Handles.Label(center + new Vector3(-cellRadius/1.5f, 0f, 0f),cellGrid[i,j].straightLeft.ToString());
                            Handles.Label(center + new Vector3(cellRadius/1.5f, 0f, 0f),cellGrid[i,j].straightRight.ToString());
                            Handles.Label(center + new Vector3(cellRadius/1.5f, cellRadius/1.5f, 0f),cellGrid[i,j].diaUR.ToString());
                            Handles.Label(center + new Vector3(-cellRadius/1.5f, -cellRadius/1.5f, 0f),cellGrid[i,j].diaDL.ToString());
                            Handles.Label(center + new Vector3(-cellRadius/1.5f, cellRadius/1.5f, 0f),cellGrid[i,j].diaUL.ToString());
                            Handles.Label(center + new Vector3(cellRadius/1.5f, -cellRadius/1.5f, 0f),cellGrid[i,j].diaDR.ToString());
                        }
                        Handles.color = Color.blue;
                        //Utilising Handles.ArrowHandleCap method to draw an arrow, and transform it by the best direction vector

                        //Draw primary jump points if enabled
                        if (primaryJumpPoints) {
                            if (cellGrid[i, j].leftForce) {
                                Handles.ArrowHandleCap( 0,cellGrid[i,j].worldPosition + new Vector3(cellRadius, 0f, 0f),transform.rotation * Quaternion.LookRotation(new Vector3(-1f, 0f, 0f)),cellRadius/1.5f,EventType.Repaint);
                            }
                            if (cellGrid[i, j].rightForce) {
                                Handles.ArrowHandleCap( 0,cellGrid[i,j].worldPosition + new Vector3(-cellRadius, 0f, 0f),transform.rotation * Quaternion.LookRotation(new Vector3(1f, 0f, 0f)),cellRadius/1.5f,EventType.Repaint);
                            }
                            if (cellGrid[i, j].upForce) {
                                Handles.ArrowHandleCap( 0,cellGrid[i,j].worldPosition + new Vector3(0f, -cellRadius, 0f),transform.rotation * Quaternion.LookRotation(new Vector3(0f, 1f, 0f)),cellRadius/1.5f,EventType.Repaint);
                            }
                            if (cellGrid[i, j].downForce) {
                                Handles.ArrowHandleCap( 0,cellGrid[i,j].worldPosition + new Vector3(0f, cellRadius, 0f),transform.rotation * Quaternion.LookRotation(new Vector3(0f, -1f, 0f)),cellRadius/1.5f,EventType.Repaint);
                            }
                        }
                    }
                }
            }
        }
    }

    //The following are helper methods, which will be used by the methods above:
    //Method which returns the Neighboring Cells, given current grid Index, and list of Vectors
    private List<Cell> GetNeighboringCells(Vector2Int cellGridIndex, List<Vectors> vectors)
    {   
        //List of Cells, which will hold the Cell neighbors of the current Cell
        List<Cell> cellNeighbors = new List<Cell>();

        //Looping through all the vectors
        foreach (Vectors currentDirection in vectors)
        {
            //Retrieving Cell Neighbor, from function
            Cell cellNeighbor = GetCellAtGridIndexPos(cellGridIndex, currentDirection.vector);
            //If cell Neighbor is not empty, we will add to the list
            if (cellNeighbor != null)
            {
                cellNeighbors.Add(cellNeighbor);
            }
        }
        //Returning list of neighbors
        return cellNeighbors;
    }

    //Method which returns the new Cell, given the current position and vector offset of new position
    public Cell GetCellAtGridIndexPos(Vector2Int currentPos, Vector2Int vectorOffset)
    {
        //Obtaining, the new position by adding the current position by vector offset
        Vector2Int newPosition = currentPos + vectorOffset;

        //Checking that the new position is a valid position, i.e., does not violate grid range
        if (newPosition.x < 0 || newPosition.x >= sizeOfGrid.x || newPosition.y < 0 || newPosition.y >= sizeOfGrid.y)
        {   
            //Invalid position, thus, null is returned
            return null;
        }
        //Valid position, thus, the Cell at the required position is returned
        else { return cellGrid[newPosition.x, newPosition.y]; }
    }

    //Method which retrieves Cell position given a Vector3 vector
    public Cell GetCellFromVector3(Vector3 vector)
    {
        //Calculating Cell x and y grid index
        //Calculating percentages of input vector's x and y values, and utilising the object's position
        float percentageX = (vector.x-transform.position.x) / (sizeOfGrid.x * cellRadius * 2);
        float percentageY = (vector.y-transform.position.y) / (sizeOfGrid.y * cellRadius * 2);
        //Clamping percentages to a value between 0 and 1
        percentageX = Mathf.Clamp01(percentageX);
        percentageY = Mathf.Clamp01(percentageY);
        //Retrieving gridIndex x and y positions, and clamping the value between 0 and size of grid -1
        int x = Mathf.Clamp(Mathf.FloorToInt((sizeOfGrid.x) * percentageX), 0, sizeOfGrid.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((sizeOfGrid.y) * percentageY), 0, sizeOfGrid.y - 1);
        //Returning Cell
        return cellGrid[x, y];
    }


    //Update Method
    private void Start(){
        //Generating Field Grid, and Cost Field
        CreateFieldGrid();
        CreateCostField();
        //Calculate jump points
        JumpPoints();
        //Retrieving mouse position
        //Note, that 10f is being used as the z-axis, as by default screento world takes the position of the camera, which in this case had a z-axis of -10f
        Vector3 mousePosition = GameObject.FindGameObjectWithTag("Tower").transform.position;
        CreateFlowField();
    }

    //Heuristic functions
    public double Manhattan(int x, int y, int a, int b) {
        return (Math.Abs(x - a)) + (Math.Abs(y - b));
    }

    public double Chebyshev(int x, int y, int a, int b) {
        return (Math.Max((Math.Abs(x - a)), (Math.Abs(y - b))));
    }

    public double Euclidean(int x, int y, int a, int b) {
        return (Math.Sqrt((Math.Pow((x - a), 2)) + (Math.Pow((y - b), 2))));
    }
}