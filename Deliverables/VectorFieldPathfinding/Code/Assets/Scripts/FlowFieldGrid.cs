using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class FlowFieldGrid : MonoBehaviour 
{
    //Variable which holds a 2d array of Cells (Grid)
    [SerializeField] private Cell [,] cellGrid;
    //Variable which holds the size of the Grid
    [SerializeField] private Vector2Int sizeOfGrid;
    //Variable which holds hte Cell Radius
    [SerializeField] private float cellRadius;
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


    //Constructor for Class
    public FlowFieldGrid(float cRadius, Vector2Int gridSize){
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
                    continue;
            }
        }
    }

    //3) Method which creates the Integration Field
    private void CreateIntegrationField(Cell inputDestinationCell)
    {   
        //Setting the inputted destination Cell, and Cost to 0 since it is the destination
        destinationCell = inputDestinationCell;
        destinationCell.cellCost = 0;
        destinationCell.bestCost = 0;

        //Creating a Queue of Cells, and running a breadth first search from the destination Cell, to update the field costs
        Queue<Cell> cellQueue = new Queue<Cell>();
        //Enqueing destination Cell
        cellQueue.Enqueue(destinationCell);
        //While Queue is not empty
        while(cellQueue.Count > 0)
        {
            //Dequeueing from Cell Queue
            Cell currentCell = cellQueue.Dequeue();
            //Retrieving the Cell Neighbors, and storing them in a list of Cells
            List<Cell> cellNeighbors = GetNeighboringCells(currentCell.gridIndex, Vectors.primaryNeighborDirections);
            //Looping through all Neighbors
            foreach (Cell neighbor in cellNeighbors)
            {
                //If cost of the neighbor Cell is max value then we will not add the Cell to the Queue
                if (neighbor.cellCost == byte.MaxValue) { continue; }
                //Checking whether to update the best cost of the neighbor, or not, and adding such neighbor to the queue
                //Note that in the Cell class, we set the best cost by default to the max ushort value.
                if (neighbor.cellCost + currentCell.bestCost < neighbor.bestCost)
                {
                    neighbor.bestCost = (ushort)(neighbor.cellCost + currentCell.bestCost);
                    cellQueue.Enqueue(neighbor);
                }
            }
        }
    }

    //4) Method which creates the Flow Field
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
                if(neighbor.bestCost < bestCost)
                {
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
                    //Drawing cubes via Vector3 for grid utilising calculated offset and current position
                    Vector3 center = new Vector3(cellRadius*2*i+cellRadius+transform.position.x,cellRadius*2*j+cellRadius+transform.position.y,transform.position.z);
                    Vector3 size = Vector3.one*cellRadius*2;
                    Gizmos.DrawWireCube(center,size);

                    //Drawing Cost Field, if drawCostField variable is set to true and mouseclicked set to true
                    if(mouseclicked &&drawCostField){
                        //Utilising Handles.Label to draw cost
                        Handles.Label(center,cellGrid[i,j].cellCost.ToString());
                    }
                    //Drawing Integration Field, if drawIntegrationField variable is set to true and mouseclicked set to true
                    if(mouseclicked &&drawIntegrationField){
                        //Utilising Handles.Label to draw best cost
                        Handles.Label(center,cellGrid[i,j].bestCost.ToString());
                    }
                    //Drawing Flow Field, if drawFlowField variable is set to true and mouseclicked set to true
                    if(mouseclicked &&drawFlowField){
                        //Checking if cell is not an obstacle
                        if(cellGrid[i,j].cellCost!=byte.MaxValue){
                            //Setting Handles color to blue
                            Handles.color = Color.blue;
                            //Constructing a Vector3 vector, for the currentCell's best direction
                            Vector3 vector= new Vector3(cellGrid[i,j].bestDirection.vector.x, cellGrid[i,j].bestDirection.vector.y, transform.position.z);
                            //Checking that the Vector is not a zero Vector
                            if(vector!=Vector3.zero){
                                //Utilising Handles.ArrowHandleCap method to draw an arrow, and transform it by the best direction vector
                                Handles.ArrowHandleCap( 0,cellGrid[i,j].worldPosition,transform.rotation * Quaternion.LookRotation(vector),cellRadius,EventType.Repaint);
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
    private Cell GetCellAtGridIndexPos(Vector2Int currentPos, Vector2Int vectorOffset)
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
    private void Update(){
        //Checking whether mouse was clicked
        if (Input.GetMouseButtonDown(0))
        {   
            //Generating Field Grid, and Cost Field
            CreateFieldGrid();
            CreateCostField();

            //Retrieving mouse position
            //Note, that 10f is being used as the z-axis, as by default screento world takes the position of the camera, which in this case had a z-axis of -10f
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            //Converting position from screen space to world space
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            
            //Retrieving Cell clicked
            Cell CellClicked = GetCellFromVector3(worldMousePosition);
            //Generating Integration Field from the Clicked Cell, and Flow Field
            CreateIntegrationField(CellClicked);
            CreateFlowField();
            //Setting mouseClicked to true
            mouseclicked=true; 
        }
    }
}
