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


    //Constructor for Class
    public Cell (Vector3 worldPos, Vector2Int gIndex){
        worldPosition=worldPos;
        gridIndex=gIndex;
        cellCost=1;//Set to default value for Cost
        bestCost = ushort.MaxValue;//Setting the bestCost to a large value, so that it could be overwritten later on
        bestDirection=Vectors.none;//Setting best direction to none
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
