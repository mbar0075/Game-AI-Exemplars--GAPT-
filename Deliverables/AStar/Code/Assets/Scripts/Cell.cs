using UnityEngine;

public class Cell
{   
    //Variable which holds Cell Vector3 position
    public Vector3 worldPosition;
    //Variable which holds Cell Vector2 grid position (for example Cell at position row 0, column 0)
    public Vector2Int gridIndex;
    //Variable to hold Cell Cost (byte has values from 0 to 255)
    public byte cellCost;
    //Variable to hold best cost (unsigned short integer)
    public ushort bestCost;
    //Variable to hold the Cell best Direction (Vector)
    public Vectors bestDirection;

    //Variables to check if the cell is a wall or a primary jump point
    public bool isWall, isPrimaryJP = false;
    //Variables to check the directions of the primary jump point
    public bool downForce, leftForce, upForce, rightForce = false;
    //Variables to store the other jump point values
    public int straightDown, straightLeft, straightUp, straightRight, diaDL, diaDR, diaUL, diaUR;

    //Constructor for Class
    public Cell (Vector3 worldPos, Vector2Int gIndex){
        worldPosition = worldPos;
        gridIndex = gIndex;
        //Set to default value for Cost
        cellCost = 1;
        //Setting the bestCost to a large value, so that it could be overwritten later on
        bestCost = ushort.MaxValue;
        //Setting best direction to none
        bestDirection=Vectors.none;
    }

    //Method, which will add an amount (parameter) to the Cell Cost
    public void AddToCost(int addAmount){
        //Error Checking to check that cost did not exceed max value for byte
        if(cellCost==byte.MaxValue){
            return;
        }
        //Max Capping Cell Cost value to 255
        if(cellCost+addAmount>=byte.MaxValue){
            cellCost=byte.MaxValue;
        }
        //Else Adding amount
        else{
            cellCost+=(byte)addAmount;
        }
    }

}
