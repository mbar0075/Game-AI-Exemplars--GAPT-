using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundGenerator : MonoBehaviour
{
    //Variables to hold min and max width
    [SerializeField] private int minwidth,maxwidth;
    //Variables to hold min and max height
    [SerializeField] private int minheight,maxheight;
    //Variable to hold the Z axis position
    [SerializeField] private int zAxis;
    //Variables to hold array of Background Tiles
    [SerializeField] private Tile [] BackgroundTiles;
    //Variable to hold tilemap
    [SerializeField] private Tilemap tilemap;

    // Start is called before the first frame update
    private void Start()
    {
        //Calling Generate Bckground Method
        GenerateBackground();
    }

    //Method which Generates a Background Tilemap, via width and height class variables and background tile
    private void GenerateBackground(){
        //Creating Background by looping through the respective Tile area in the tilemap, and setting the respective tiles
        for(int x=minwidth; x<maxwidth;x++){
            for(int y=minheight; y<maxheight;y++){
                tilemap.SetTile(new Vector3Int(x,y,zAxis),GetRandomTile());
            }
        }
    }

    //Method which returns a random Tile fromm the BackgroundTiles array
    private Tile GetRandomTile(){
        return BackgroundTiles[Random.Range(0,BackgroundTiles.Length)];
    }
}