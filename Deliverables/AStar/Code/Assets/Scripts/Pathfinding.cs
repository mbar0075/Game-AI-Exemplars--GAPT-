using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    //Declaring variables and components to be used
    private Transform target;
	private MainGrid tempGrid;
	private SetHeuristic setHeuristic;
    private List<PathNode> openList;
	private List<PathNode> closedList;
    private List<Vector3> pathToFollow;
    private Vector3 startPosition;
    private PathGrid nodeGrid;
    private bool pathFound = false;
    private int waypointCounter = 0;


    private void Start() {
		//Initialising components
        target = GameObject.FindGameObjectWithTag("Tower").GetComponent<Transform>();
        tempGrid = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MainGrid>();
        setHeuristic = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SetHeuristic>();
		//Set nodeGrid to a new pathgrid, passing the tempGrid's width and height as well as a Vector3.zero
        nodeGrid = new PathGrid(tempGrid.sizeOfGrid.x, tempGrid.sizeOfGrid.y, tempGrid.cellRadius, new Vector3(0, 0, 0));
		//Call FindPath and save the returned list of vectors in pathToFollow
        pathToFollow = FindPath();
		//Set startPosition to the current position's cell's grid index vector
        startPosition = new Vector3(nodeGrid.grid.GetCellFromVector3(transform.position).gridIndex.x, nodeGrid.grid.GetCellFromVector3(transform.position).gridIndex.y, 0);
	}

	//Function to calculate the path to be taken by the game object to reach the target
    public List<Vector3> FindPath() {
		//Setting the start and end coordinates of the path
        int startX = nodeGrid.grid.GetCellFromVector3(this.transform.position).gridIndex.x;
        int startY = nodeGrid.grid.GetCellFromVector3(this.transform.position).gridIndex.y;
        int endX = nodeGrid.grid.GetCellFromVector3(target.position).gridIndex.x;
        int endY = nodeGrid.grid.GetCellFromVector3(target.position).gridIndex.y;

		//Call FindPath by passing the start and end coordinates, save returned list of pathnodes in path
		List<PathNode> path = FindPath(startX, startY, endX, endY);

		//If the path returned is null, return null
		//Otherwise, convert the list of pathnodes to a list of Vector3, return the new list and set pathFound to true
		if (path == null) {
			return null;
		} else {
			List<Vector3> vectorPath = new List<Vector3>();
			foreach (PathNode pathNode in path) {
				vectorPath.Add(new Vector3(pathNode.cell.gridIndex.x, pathNode.cell.gridIndex.y) + new Vector3(0.5f, 0.5f, 0f));
			}
            pathFound = true;
            return vectorPath;
		}
	}

	//Function to calculate the path of path nodes from a starting x and y to an ending x and y
    private List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
		//Get the pathnodes of the passed coordinates
		PathNode startNode = nodeGrid.gridArray[startX, startY];
		PathNode endNode = nodeGrid.gridArray[endX, endY];
	
		//Set the openList initially to only contain the start node, the list will store the explored nodes
		openList = new List<PathNode> { startNode };
		//Create a closedList to store the explored nodes
		closedList = new List<PathNode>();

		//For each path node, calculate its fcost and set its cameFromNode to null
		for (int x = 0; x < nodeGrid.grid.sizeOfGrid.x; x++) {
			for (int y = 0; y < nodeGrid.grid.sizeOfGrid.y; y++) {
				PathNode pathNode = nodeGrid.gridArray[x, y];
				pathNode.gCost = int.MaxValue;
				pathNode.CalculateFCost();
				pathNode.cameFromNode = null;
			}
		}

		//Set the start node's gcost to zero, calculate its hcost and its fcost
		startNode.gCost = 0;
		startNode.hCost = CalculateDistanceCost(startNode, endNode);
		startNode.CalculateFCost();

		//Loop until the openList is empty
		while (openList.Count > 0) {
			//Get the path node with the lowest fcost from the openList and save it in currentNode
			PathNode currentNode = GetLowestFCostNode(openList);

			//If the currentNode is the end node call the CalculatePath function and return its result
			if (currentNode == endNode) {
				return CalculatePath(endNode);
			}

			//Remove current node from the open list and add it to the closed list
			openList.Remove(currentNode);
			closedList.Add(currentNode);

            PathNode tempNode;
			//For each cell's jump point direction, if it is greater than zero, calculate its temptentativeGCost by adding the current node's gCost to the heuristic cost 
			//to get from the current node to the primary jump point in that direction. If it is smaller than the current node's gcost, set the tempNode's cameFromNode 
			//to the current node, set its gCost to temptentativeGCost and calculate its hCost and fCost, if the node is not in the openList, add it
			//This functionality is repeated for all eight directions
            if (currentNode.cell.straightDown > 0) {
                int temptentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, nodeGrid.gridArray[currentNode.cell.gridIndex.x, currentNode.cell.gridIndex.y - currentNode.cell.straightDown]);
                tempNode = nodeGrid.gridArray[currentNode.cell.gridIndex.x, currentNode.cell.gridIndex.y - currentNode.cell.straightDown];
                if (temptentativeGCost < tempNode.gCost) {
                    tempNode.cameFromNode = currentNode;
				    tempNode.gCost = temptentativeGCost;
				    tempNode.hCost = CalculateDistanceCost(tempNode, endNode);
				    tempNode.CalculateFCost();

				    if (!openList.Contains(tempNode)) {
					    openList.Add(tempNode);
				    }
                }
            }
            if (currentNode.cell.straightUp > 0) {
                int temptentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, nodeGrid.gridArray[currentNode.cell.gridIndex.x, currentNode.cell.gridIndex.y + currentNode.cell.straightUp]);
                tempNode = nodeGrid.gridArray[currentNode.cell.gridIndex.x, currentNode.cell.gridIndex.y + currentNode.cell.straightUp];
                if (temptentativeGCost < tempNode.gCost) {
                    tempNode.cameFromNode = currentNode;
				    tempNode.gCost = temptentativeGCost;
				    tempNode.hCost = CalculateDistanceCost(tempNode, endNode);
				    tempNode.CalculateFCost();

				    if (!openList.Contains(tempNode)) {
					    openList.Add(tempNode);
				    }
                }
            }
            if (currentNode.cell.straightLeft> 0) {
                int temptentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, nodeGrid.gridArray[currentNode.cell.gridIndex.x - currentNode.cell.straightLeft, currentNode.cell.gridIndex.y]);
                tempNode = nodeGrid.gridArray[currentNode.cell.gridIndex.x - currentNode.cell.straightLeft, currentNode.cell.gridIndex.y];
                if (temptentativeGCost < tempNode.gCost) {
                    tempNode.cameFromNode = currentNode;
				    tempNode.gCost = temptentativeGCost;
				    tempNode.hCost = CalculateDistanceCost(tempNode, endNode);
				    tempNode.CalculateFCost();

				    if (!openList.Contains(tempNode)) {
					    openList.Add(tempNode);
				    }
                }
            }
            if (currentNode.cell.straightRight > 0) {
                int temptentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, nodeGrid.gridArray[currentNode.cell.gridIndex.x + currentNode.cell.straightRight, currentNode.cell.gridIndex.y]);
                tempNode = nodeGrid.gridArray[currentNode.cell.gridIndex.x + currentNode.cell.straightRight, currentNode.cell.gridIndex.y];
                if (temptentativeGCost < tempNode.gCost) {
                    tempNode.cameFromNode = currentNode;
				    tempNode.gCost = temptentativeGCost;
				    tempNode.hCost = CalculateDistanceCost(tempNode, endNode);
				    tempNode.CalculateFCost();

				    if (!openList.Contains(tempNode)) {
					    openList.Add(tempNode);
				    }
                }
            }
            if (currentNode.cell.diaUR > 0) {
                int temptentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, nodeGrid.gridArray[currentNode.cell.gridIndex.x + currentNode.cell.diaUR, currentNode.cell.gridIndex.y + currentNode.cell.diaUR]);
                tempNode = nodeGrid.gridArray[currentNode.cell.gridIndex.x + currentNode.cell.diaUR, currentNode.cell.gridIndex.y + currentNode.cell.diaUR];
                if (temptentativeGCost < tempNode.gCost) {
                    tempNode.cameFromNode = currentNode;
				    tempNode.gCost = temptentativeGCost;
				    tempNode.hCost = CalculateDistanceCost(tempNode, endNode);
				    tempNode.CalculateFCost();

				    if (!openList.Contains(tempNode)) {
					    openList.Add(tempNode);
				    }
                }
            }
            if (currentNode.cell.diaUL > 0) {
                int temptentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, nodeGrid.gridArray[currentNode.cell.gridIndex.x - currentNode.cell.diaUL, currentNode.cell.gridIndex.y + currentNode.cell.diaUL]);
                tempNode = nodeGrid.gridArray[currentNode.cell.gridIndex.x - currentNode.cell.diaUL, currentNode.cell.gridIndex.y + currentNode.cell.diaUL];
                if (temptentativeGCost < tempNode.gCost) {
                    tempNode.cameFromNode = currentNode;
				    tempNode.gCost = temptentativeGCost;
				    tempNode.hCost = CalculateDistanceCost(tempNode, endNode);
				    tempNode.CalculateFCost();

				    if (!openList.Contains(tempNode)) {
					    openList.Add(tempNode);
				    }
                }
            }
            if (currentNode.cell.diaDR > 0) {
                int temptentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, nodeGrid.gridArray[currentNode.cell.gridIndex.x + currentNode.cell.diaDR, currentNode.cell.gridIndex.y - currentNode.cell.diaDR]);
                tempNode = nodeGrid.gridArray[currentNode.cell.gridIndex.x + currentNode.cell.diaDR, currentNode.cell.gridIndex.y - currentNode.cell.diaDR];
                if (temptentativeGCost < tempNode.gCost) {
                    tempNode.cameFromNode = currentNode;
				    tempNode.gCost = temptentativeGCost;
				    tempNode.hCost = CalculateDistanceCost(tempNode, endNode);
				    tempNode.CalculateFCost();

				    if (!openList.Contains(tempNode)) {
					    openList.Add(tempNode);
				    }
                }
            }
            if (currentNode.cell.diaDL > 0) {
                int temptentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, nodeGrid.gridArray[currentNode.cell.gridIndex.x - currentNode.cell.diaDL, currentNode.cell.gridIndex.y - currentNode.cell.diaDL]);
                tempNode = nodeGrid.gridArray[currentNode.cell.gridIndex.x - currentNode.cell.diaDL, currentNode.cell.gridIndex.y - currentNode.cell.diaDL];
                if (temptentativeGCost < tempNode.gCost) {
                    tempNode.cameFromNode = currentNode;
				    tempNode.gCost = temptentativeGCost;
				    tempNode.hCost = CalculateDistanceCost(tempNode, endNode);
				    tempNode.CalculateFCost();

				    if (!openList.Contains(tempNode)) {
					    openList.Add(tempNode);
				    }
                }
            }

			//For each path node in the neighbourList of the currentNode
			foreach (PathNode neighbourNode in GetNeighbours(currentNode)) {
				//If the path node is already in the closed list, continue
				//If the path node is a wall, add it to the closed list and continue

				if (closedList.Contains(neighbourNode)) {
					continue;
				} 
				if (neighbourNode.cell.isWall) {
					closedList.Add(neighbourNode);
					continue;
				}

				//Set tentativeGCost to the currentNode's gCost added with the calculated heuristic distance from the current node to the neighbour node
				int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
				//If the tentativeGCost is less than the neighbourNode's gCost, set neighbourNode's cameFromNode to the currentNode, set its gcost to tentativeGCost
				//Set its hCost to the calculated heuristic distance and calculate its fCost
				if (tentativeGCost < neighbourNode.gCost) {
					neighbourNode.cameFromNode = currentNode;
					neighbourNode.gCost = tentativeGCost;
					neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
					neighbourNode.CalculateFCost();
					//If the neighbourNode is not in the openList, add it
					if (!openList.Contains(neighbourNode)) {
						openList.Add(neighbourNode);
					}
				}
			}
		}

		return null;
	}

	//Function that returns a list of path nodes of the neighbours of a given pathnode (surrounding 8 path nodes)
    private List<PathNode> GetNeighbours(PathNode currentNode) {
		List<PathNode> neighbourList = new List<PathNode>();
		//Check if the node to be added is within the grid, if so add it to neighbourList
		if (currentNode.x - 1 >= 0) {
			neighbourList.Add(nodeGrid.gridArray[currentNode.x - 1, currentNode.y]);

			if (currentNode.y - 1 >= 0) neighbourList.Add(nodeGrid.gridArray[currentNode.x - 1, currentNode.y - 1]);

			if (currentNode.y + 1 < nodeGrid.grid.sizeOfGrid.y) neighbourList.Add(nodeGrid.gridArray[currentNode.x - 1, currentNode.y + 1]);
		}
		if (currentNode.x + 1 < nodeGrid.grid.sizeOfGrid.x) {
			neighbourList.Add(nodeGrid.gridArray[currentNode.x + 1, currentNode.y]);

			if (currentNode.y - 1 >= 0) neighbourList.Add(nodeGrid.gridArray[currentNode.x + 1, currentNode.y - 1]);

			if (currentNode.y + 1 < nodeGrid.grid.sizeOfGrid.y) neighbourList.Add(nodeGrid.gridArray[currentNode.x + 1, currentNode.y + 1]);
		}

		if (currentNode.y - 1 >= 0) neighbourList.Add(nodeGrid.gridArray[currentNode.x, currentNode.y - 1]);

		if (currentNode.y + 1 < 0) neighbourList.Add(nodeGrid.gridArray[currentNode.x, currentNode.y + 1]);

		//Return neighbourList
		return neighbourList;
	}

	//Function to retrieve the list of pathnodes after completing the A* search
    private List<PathNode> CalculatePath(PathNode endNode) {
		//Traverse the path nodes from the end node to the start node, by checking the cameFromNode variable of each path node
		List<PathNode> path = new List<PathNode>();
		path.Add(endNode);
		PathNode currentNode = endNode;
		while (currentNode.cameFromNode != null) {
			path.Add(currentNode.cameFromNode);
			currentNode = currentNode.cameFromNode;
		}
		//Reverse the path so that it starts from the startnode and ends with the end node, and return the list
		path.Reverse();
		return path;
	}

	//Function to calculate the heuristic cost of two pathnodes
	private int CalculateDistanceCost(PathNode a, PathNode b) {
		//Depending on the current setHeuristic boolean variable set to true, call the appropriate function and return it
        if (setHeuristic.manhattan) {
            return (int)nodeGrid.grid.Manhattan(a.x, a.y, b.x, b.y);
        } else if (setHeuristic.chebyshev) {
            return (int)nodeGrid.grid.Chebyshev(a.x, a.y, b.x, b.y);
        } else if (setHeuristic.euclidean) {
            return (int)nodeGrid.grid.Euclidean(a.x, a.y, b.x, b.y);
        }
        Debug.Log("Error");
        return -1;
	}

	//Function to get the PathNode with the lowest fcost
	private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) {
		PathNode lowestFCostNode = pathNodeList[0];
		//Iterate through every pathNode, if the current node's fcost is less than the current lowestFCostNode, replace it
		for (int i = 1; i < pathNodeList.Count; i++) {
			if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
				lowestFCostNode = pathNodeList[i];
			}
		}
		return lowestFCostNode;
	}

    void Update() {
		//If a path exists
        if (pathFound) {
			//If the game object has reached the current Vector3 in pathToFollow, increment the waypointCounter
            if (transform.position == pathToFollow[waypointCounter]) {
                waypointCounter++;
			//Otherwise, move towards the next waypoint
            } else {
                Vector3 temp = Vector3.MoveTowards(transform.position,pathToFollow[waypointCounter],1f * Time.deltaTime);
                transform.position = temp;
            }
        }
    }

	//Draw the path as a red line
    private void OnDrawGizmos(){
        if (pathFound) {
            for (int i = 1; i < pathToFollow.Count; i++) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(pathToFollow[i-1], pathToFollow[i]);
            }
        }
    }
}
