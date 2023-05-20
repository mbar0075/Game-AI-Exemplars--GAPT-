using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSP : MonoBehaviour
{
    public class Room
    {
        public float height, width;
        public Vector3 position;
    }

    public class Node
    {
        public float x, y, height, width;
        public Vector3 position;
        public bool isLeaf, horizontal, corridorLeft = false, corridorRight = false, corridorUp = false, corridorDown = false;
        public Node left, right;
        public Room room;
        public GameObject roomObject;
    }

    public int minRoomWidth, minRoomHeight, maxRoomWidth, maxRoomHeight, minX, minY, maxX, maxY;
    public GameObject floorTile, corridorTile, playerTile, backgroundTile;
    private Node root;
    private Node room2;
    private RaycastHit2D hit;
    bool playerSpawn = false;
    float roomHeight, roomWidth;
    List<Vector3> roomCenters = new List<Vector3>();
    List<Vector3> corridorCenters = new List<Vector3>();
    Vector3 corridorPos;
    bool horizontal, valid;
    List<Node> rooms = new List<Node>();
    Vector3[] directions = { Vector3.left, Vector3.right, Vector3.up, Vector3.down };

    void Start()
    {
        root = new Node();
        root.x = (minX + maxX) / 2;
        root.y = (minY + maxY) / 2;
        root.position = new Vector3(root.x, root.y, 0);

        horizontal = (Random.value > 0.5f);
        root.horizontal = horizontal;

        root.height = (maxY - minY);
        root.width = (maxX - minX);

        root.isLeaf = false;
        Split(root, horizontal);
        valid = true;
    }

    void Update()
    {
        if (valid)
        {
            foreach (Node room in rooms)
            {
                foreach (Vector3 direction in directions)
                {
                    hit = Physics2D.Raycast(room.position, direction.normalized);

                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.name != "Corridor(Clone)")
                        {
                            foreach (Node temp in rooms)
                            {
                                if (temp.roomObject.transform.position == hit.collider.transform.position)
                                {
                                    room2 = temp;
                                    break;
                                }
                            }

                            if (direction == Vector3.left && !room.corridorLeft && !room2.corridorRight)
                            {
                                CreateCorridors(room, direction, hit);
                                room.corridorLeft = true;
                                room2.corridorRight = true;
                            } else if (direction == Vector3.right && !room.corridorRight && !room2.corridorLeft)
                            {
                                CreateCorridors(room, direction, hit);
                                room.corridorRight = true;
                                room2.corridorLeft = true;
                            } else if (direction == Vector3.up && !room.corridorUp && !room2.corridorDown)
                            {
                                CreateCorridors(room, direction, hit);
                                room.corridorUp = true;
                                room2.corridorDown = true;
                            } else if (direction == Vector3.down && !room.corridorDown && !room2.corridorUp)
                            {
                                CreateCorridors(room, direction, hit);
                                room.corridorDown = true;
                                room2.corridorUp = true;
                            }
                        }
                    }
                }
            }
            SpawnPlayer();
            valid = false;
        }
    }

    void Split(Node node, bool horizontal)
    {
        if (!node.isLeaf)
        {
            float newHeight, newWidth, heightDiff, widthDiff;

            if (node.horizontal)
            {
                //Horizontal Split

                newHeight = Random.Range(maxRoomHeight, (node.height - maxRoomHeight));
                heightDiff = node.height - newHeight;

                node.left = new Node();
                node.left.height = newHeight;
                node.left.width = node.width;
                node.left.position = new Vector3(node.position.x, (node.position.y + ((node.height / 2) - newHeight / 2)), 0);
                node.left.isLeaf = checkDimensions(node.left);

                node.right = new Node();
                node.right.height = heightDiff;
                node.right.width = node.width;
                node.right.position = new Vector3(node.position.x, (node.position.y - ((node.height / 2) - heightDiff / 2)), 0);
                node.right.isLeaf = checkDimensions(node.right);

                horizontal = false;
            }
            else
            {
                //Vertical Split

                newWidth = Random.Range(maxRoomWidth, (node.width - maxRoomWidth));
                widthDiff = node.width - newWidth;

                node.left = new Node();
                node.left.height = node.height;
                node.left.width = newWidth;
                node.left.position = new Vector3((node.position.x - ((node.width / 2) - newWidth / 2)), node.position.y, 0);
                node.left.isLeaf = checkDimensions(node.left);

                node.right = new Node();
                node.right.height = node.height;
                node.right.width = widthDiff;
                node.right.position = new Vector3((node.position.x + ((node.width / 2) - widthDiff / 2)), node.position.y, 0);
                node.right.isLeaf = checkDimensions(node.right);

                horizontal = true;
            }

            if (!node.left.isLeaf)
            {
                //Splitting Left Node
                Split(node.left, horizontal);
            }
            else
            {
                //Creating Room
                CreateRoom(node.left);
            }

            if (!node.right.isLeaf)
            {
                //Splitting Right Node
                Split(node.right, horizontal);
            }
            else
            {
                //Creating Room
                CreateRoom(node.right);
            }
        }
    }

    bool checkDimensions(Node node)
    {
        if ((node.height < (maxRoomHeight*1.5)) && (node.width < (maxRoomWidth*1.5)))
        {
            //The node is too small to split further.
            return true;
        } else if (node.height < (maxRoomHeight*1.5))
        {
            node.horizontal = false;
        } else if (node.width < (maxRoomWidth*1.5))
        {
            node.horizontal = true;
        }
        return false;
    }

    void CreateRoom(Node node)
    {
        if (node.isLeaf)
        {
            node.room = new Room();

            if(node.height < maxRoomHeight)
            {
                roomHeight = Random.Range(minRoomHeight, node.height - 3);
            } else
            {
                roomHeight = Random.Range(minRoomHeight, maxRoomHeight);
            }

            if (node.width < maxRoomWidth)
            {
                roomWidth = Random.Range(minRoomWidth, node.width - 3);
            }
            else
            {
                roomWidth = Random.Range(minRoomWidth, maxRoomWidth);
            }

            node.room.height = roomHeight;
            node.room.width = roomWidth;
            node.room.position = node.position;

            rooms.Add(node);
            roomCenters.Add(node.position);

            GameObject background = Instantiate(backgroundTile, node.position, Quaternion.identity);
            GameObject room = Instantiate(floorTile, node.position, Quaternion.identity);

            room.transform.localScale = new Vector3(roomWidth, roomHeight, 0);
            background.transform.localScale = new Vector3(node.width, node.height, 0);
       
            node.roomObject = room;
        }
    }

    void CreateCorridors(Node room, Vector3 direction, RaycastHit2D hit)
    {
        GameObject corridor;
        float height;
        float width;
        Vector3 position;
        position = Vector3.Lerp(room.position, hit.point, 0.5f);
        corridorCenters.Add(position);

        corridor = Instantiate(corridorTile, position, Quaternion.identity);

        if (direction == Vector3.right || direction == Vector3.left)
        {
            width = Mathf.Abs(hit.collider.transform.position.x - room.position.x);

            corridor.transform.localScale = new Vector3(
                width,
                4,
                corridor.transform.localScale.z);
        }
        else if (direction == Vector3.up || direction == Vector3.down)
        {
            height = Mathf.Abs(hit.collider.transform.position.y - room.position.y);

            corridor.transform.localScale = new Vector3(4,
                height,
                corridor.transform.localScale.z);
        }
    }

    void SpawnPlayer()
    {
        GameObject player;
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();

        if (!playerSpawn)
        {
            player = Instantiate(playerTile, rooms[0].position, Quaternion.identity);
            playerSpawn = true;
            camera.target = player.transform;
        }
    }
}