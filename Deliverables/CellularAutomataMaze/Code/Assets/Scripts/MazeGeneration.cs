using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGeneration : MonoBehaviour
{
    //Variables to hold min and max width
    [SerializeField] private int minwidth,maxwidth;
    //Variables to hold min and max height
    [SerializeField] private int minheight,maxheight;
    //Variable to hold the Z axis position
    [SerializeField] private int zAxis;
    //Variables to hold Obstacle Tile
    [SerializeField] private Tile ObstcaleTile;
    //Variable to hold tilemap
    [SerializeField] private Tilemap tilemap;
    //Variable to act as a stopping condition, i.e., continue to loop until a specified number of iterations, 
    //ideally value should be a high value
    [SerializeField] private int iterations;
    //Variable to hold the UpperLimit of live Neighbors, living Cells need to have
    //Note that 5 is used for the mazectric algorithm and 4 is used for the mice algorithm
    [SerializeField] private int UpperLimit;
    //2d array of integers, acting as our current Generation, 
    //whereby every Cell contains an integer state (0 signifies dead state and 1 signifies living state)
    private int[,] CurrentGenerationGrid;
    //2d array of integers, acting as our next Generation, 
    //whereby every Cell contains an integer state (0 signifies dead state and 1 signifies living state)
    private int[,] NextGenerationGrid;
    //Variable which holds the number of rows in Cell Grid
    private int rowsInGrid;
    //Variable which holds the number of columns in Cell Grid
    private int colsInGrid;
    //Variable to act as a counter
    private int count=0;
    //Variable which is used as a flag to determine, whether maze was generated
    public bool mazeGenerated=false;
    

    // Start is called before the first frame update
    private void Start()
    {   
        //Calculating the rows and columns in the Grid via respective variables, and Abs() function
        rowsInGrid=(Mathf.Abs(maxwidth)+Mathf.Abs(minwidth));
        colsInGrid=(Mathf.Abs(maxheight)+Mathf.Abs(minheight));

        //Declaring Grid
        CurrentGenerationGrid= new int[rowsInGrid,colsInGrid];
        NextGenerationGrid= new int[rowsInGrid,colsInGrid];
        //Calling Method which Initialises Grids
        InitialiseGrids();
        //Calling Method which Populates the current Generation with live Cells
        PopulateRandomLiveCells();

    }

    // Update is called once per frame
    private void Update(){
        //Calling the NextGeneration method until count< iterations, and incrementing iterations
        if(count<iterations){
            NextGeneration();
            count++;
        }
        else{
            //Setting flag to true
            mazeGenerated=true;
        }
    }

    //Method which Initialises Cells in both Grids
    private void InitialiseGrids(){
        //Looping, through all the Cells in both Grids, and initialising Cells, to dead Cells (signified by a 0)
        for(int x=0; x<rowsInGrid; x++){
            for(int y=0; y<colsInGrid; y++){
                CurrentGenerationGrid[x,y]=0;
                NextGenerationGrid[x,y]=0;
            }
        }
    }

    //Method which Populates the Current Generation with Live Cells
    private void PopulateRandomLiveCells(){
        //Looping, through the current Generation Grid's 6x6 middle position
        for(int x=(int)(rowsInGrid/2)-3; x<(int)(rowsInGrid/2)+3; x++){
            for(int y=(int)(colsInGrid/2)-3; y<(int)(colsInGrid/2)+3; y++){
                //Setting the State of the middle Cells, via Random Range
                CurrentGenerationGrid[x,y]=Random.Range(0,2);
            }
        }
    }

    //Method, which given an x and y index, will determine whether Cell will live in the next Generation
    private bool CellLivesNextGen(int x, int y){
        //CLooping through the current Cell's 8 neighbors
        int liveCellCount=0;
        for(int xoffset=-1; xoffset<2; xoffset++){
            for(int yoffset=-1; yoffset<2; yoffset++){
                //Checking, whether the Cell position is Valid and that the Current Neighbor, is in the alive state, if so will proceed to increment liveCellCount
                if(isPositionValid(x+xoffset,y+yoffset)&&CurrentGenerationGrid[x+xoffset,y+yoffset]==1){
                    liveCellCount++;
                }
            }
        }
        //The Cell lives, if it is in the living state, and has between 2 and the upper limit of live neighbors, thus method returns true
        if(CurrentGenerationGrid[x,y]==1 && (1<=liveCellCount&&liveCellCount<=UpperLimit)){
            return true;
        }
        //The Cell also lives, if is in the dead state, and has 3 live neighbors, thus method returns true
        if(CurrentGenerationGrid[x,y]==0 && liveCellCount==3){
            return true;
        }
        //For all other exceptions, method will return false
        return false;
    }

    //Method which Creates the Next Generation, upon the Current Generation
    private void NextGeneration(){
        //Looping through all the Cells in the CurrentGeneration Grid
        for(int x=0; x<rowsInGrid; x++){
            for(int y=0; y<colsInGrid; y++){
                //Checking whether current Cell is alive or dead, and depending on the outcome,
                //method will set the Tile on the tilemap to the respective tile
                if(CurrentGenerationGrid[x,y]==1){
                    //Adding the minwidth and minheight offset to the x and y values respectively, to obtain correct position
                    tilemap.SetTile(new Vector3Int(x+minwidth, y+minheight, zAxis),ObstcaleTile);
                }
                else{
                    //Adding the minwidth and minheight offset to the x and y values respectively, to obtain correct position
                    tilemap.SetTile(new Vector3Int(x+minwidth, y+minheight, zAxis),null);
                }
                //Checking whether the Current Cell, will live in the Next Generation, if so respective state is set in the NextGeneration Grid
                if(CellLivesNextGen(x,y)){
                    NextGenerationGrid[x,y]=1;
                }
                else{
                    NextGenerationGrid[x,y]=0;
                }
            }
        }
        //Overwriting the CurrentGeneration Grid, with the NextGeneration Grid
        CurrentGenerationGrid=NextGenerationGrid.Clone() as int[,];
    }

    //Method which checks, whether the given x and y index position is valid
    private bool isPositionValid(int x, int y){
        //If x and y index fall into the respective ranges, then true is returned, else false is returned
        if(0<=x && x<rowsInGrid && 0<=y&&y<colsInGrid){
            return true;
        }
        return false;
    }

}
