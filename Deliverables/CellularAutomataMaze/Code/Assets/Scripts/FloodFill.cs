using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloodFill : MonoBehaviour
{
    //Variables to hold min and max width
    [SerializeField] private int minwidth,maxwidth;
    //Variables to hold min and max height
    [SerializeField] private int minheight,maxheight;
    //Variable to hold the Z axis position
    [SerializeField] private int zAxis;
    //Variable to hold the instance of the MazeGeneration class
    [SerializeField] private MazeGeneration maze;
    //Variables to hold Draw Tile
    [SerializeField] private Tile DrawTile;
    //Variables to hold Target Tile
    [SerializeField] private Tile TargetTile;
    //Variable to hold the Flood Fill Tilemap
    [SerializeField] private Tilemap floodfilltilemap;
    //Variable to hold the Maze tilemap
    [SerializeField] private Tilemap mazetilemap;
    //Variable to hold the amount of seconds to wait
    [SerializeField] private float secondsToWait;
    //Variable which is used as a flag to determine, whether flood fill algorithm was executed
    private bool floodfilldone=false;


    // Start is called before the first frame update
    void Update()
    {   
        //Checking whether the maze generated flag is set, and that the fllod fill algorithm, was not executed
        if(maze.mazeGenerated&&!floodfilldone){
            //Setting floodfilldone flag to true, and calling the Initiate Flood Fill Algorithm method
            floodfilldone=true;
            InitiateFloodFillAlgorithm();
        }
        
    }

    //Method which Initiates the Flood Fill Algorithm, from the area boundaries
    private void InitiateFloodFillAlgorithm(){
        //A sequence of for loops, which check whether the edge tile of the maze is not the Target Tile, if so, method will proceed to
        //call the Flood Fill Algorithm from such position
            for(int x =minwidth;x<maxwidth;x++){
                Vector3Int startingpos=new Vector3Int(x,minheight,zAxis);
                if(mazetilemap.GetTile(startingpos)!=TargetTile){
                    FloodFillAlgorithm(startingpos);
                }
            }
            for(int x =minwidth;x<maxwidth;x++){
                Vector3Int startingpos=new Vector3Int(x,maxheight,zAxis);
                if(mazetilemap.GetTile(startingpos)!=TargetTile){
                    FloodFillAlgorithm(startingpos);
                }
            }
            for(int y =minheight;y<maxheight;y++){
                Vector3Int startingpos=new Vector3Int(minwidth,y,zAxis);
                if(mazetilemap.GetTile(startingpos)!=TargetTile){
                    FloodFillAlgorithm(startingpos);
                }
            }
            for(int y =minheight;y<maxheight;y++){
                Vector3Int startingpos=new Vector3Int(maxwidth,y,zAxis);
                if(mazetilemap.GetTile(startingpos)!=TargetTile){
                    FloodFillAlgorithm(startingpos);
                }
            }
    }

    //Method which performs the Flood Fill Algorithm, given a Vector3Int position
    private void FloodFillAlgorithm(Vector3Int pos){
        //Checking whether position.x and position.y values are valid positions, if not function will return
        if(pos.x<minwidth||pos.x>maxwidth||pos.y<minheight||pos.y>maxheight){
            return;
        }
        //Checking, whether the current tile position on the maze tilemap is a Target Tile,
        //and that the current tile position on the floodfill tilemap is a Draw Tile, if so function will return
        if(mazetilemap.GetTile(pos)==TargetTile||floodfilltilemap.GetTile(pos)==DrawTile){
            return;
        }
        //Calling the Recursive Call Coroutine
        StartCoroutine(RecursiveCall(pos));
        
    }

    //Coroutine, which sets the floodfilltilemap tile, and recursively calls FloodFillAlgorithm method
    IEnumerator RecursiveCall(Vector3Int pos)
    {
        //Setting the current tile, at the specified position to the Draw Tile on the floodfill tilemap
        floodfilltilemap.SetTile(pos,DrawTile);
        //Waiting for 0.5 seconds
        yield return new WaitForSeconds (secondsToWait);
        
        //Recursively calling the Flood Fill Algorithm on the 4 cardinal directions relative to the current position
        FloodFillAlgorithm(new Vector3Int(pos.x-1,pos.y,pos.z));
        FloodFillAlgorithm(new Vector3Int(pos.x+1,pos.y,pos.z));
        FloodFillAlgorithm(new Vector3Int(pos.x,pos.y-1,pos.z));
        FloodFillAlgorithm(new Vector3Int(pos.x,pos.y+1,pos.z));
    }
}