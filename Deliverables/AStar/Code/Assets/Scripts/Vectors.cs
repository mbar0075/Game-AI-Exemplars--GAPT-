using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vectors
{
    //Variable to hold the vector direction (Vector2 Int)
    public Vector2Int vector;

    //Constructor for Class
    public Vectors(int x, int y)
    {
        vector = new Vector2Int(x, y);
    }

    //Static Variables, which hold the 9 different vector types, a cell could have 
    public static readonly Vectors  none = new Vectors(0, 0);
    public static readonly Vectors  up = new Vectors (0, 1);
    public static readonly Vectors  down = new Vectors (0, -1);
    public static readonly Vectors  right = new Vectors (1, 0);
    public static readonly Vectors  left = new Vectors (-1, 0);
    public static readonly Vectors  topRight = new Vectors (1, 1);
    public static readonly Vectors  topLeft = new Vectors (-1, 1);
    public static readonly Vectors  bottomRight = new Vectors (1, -1);
    public static readonly Vectors  bottomLeft = new Vectors (-1, -1);

    //Static Variable which holds a list of the primary 4 cell neighbor directions
    public static readonly List<Vectors> primaryNeighborDirections = new List<Vectors>{up, down, left, right };

    //Static Variable which holds a list of the 8 cell neighbor directions
    public static readonly List<Vectors> allNeighborDirections = new List<Vectors>{up, down, left, right, topLeft, topRight, bottomLeft, bottomRight };

    //Static Variable which holds a list of all the directions
    public static readonly List<Vectors> allDirections = new List<Vectors>{none, up, down, left, right, topLeft, topRight, bottomLeft, bottomRight };
}
