using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waypoint : MonoBehaviour
{
    public Transform[] points;
    public float speed = 0.01f;

    private RaycastHit2D hit;
    private Vector3 target;
    private Vector3 destination;
    private int currentWaypoint;
    private int targetWaypoint;
    private Stack<int> path;
    private bool arrive;

    int[,] paths;
    int item = 0;
    Vector3 targetPosition;

    void Start()
    {
        CreateLookupTable();
        arrive = true;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentWaypoint = FindTargetWaypoint(transform.position + new Vector3(1f, 1f));
            targetWaypoint = FindTargetWaypoint(target);
            CreatePath();
        }
    }

    void FixedUpdate()
    {
        if (!arrive)
        {
            if (item == 0)
            {
                item = path.Pop();
            }

            if (transform.position != points[item].position)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                        points[item].position,
                        speed * Time.deltaTime);
            }
            else if (transform.position == target)
            {
                arrive = true;
            }
            else
            {
                if (path.Count == 0)
                {
                    item = 0;
                    transform.position = Vector3.MoveTowards(transform.position,
                        target,
                        speed * Time.deltaTime);
                    arrive = true;
                }
                else
                {
                    item = path.Pop();
                }
            }
        }
    }

    void CreateLookupTable()
    {
        paths = new int[13, 13];
        Vector3 direction;
        int i = 0;
        int j = 0;

        foreach (Transform start in points)
        {
            foreach (Transform end in points)
            {
                if (start.position == end.position)
                {
                    paths[i, j] = j;
                }
                else
                {
                    direction = (end.position - start.position).normalized;
                    hit = Physics2D.Raycast(start.position, direction);
                    if (hit.collider.gameObject.name == j.ToString())
                    {
                        paths[i, j] = j;
                    }
                    else
                    {
                        if (i < j)
                        {
                            for (int k = j - 1; k >= 0; k--)
                            {
                                direction = (end.position - points[k].position).normalized;
                                hit = Physics2D.Raycast(points[k].position, direction);

                                if (hit.collider.gameObject.name == j.ToString())
                                {
                                    paths[i, j] = k;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (i < 10 || j <= 6 || j >= 10)
                            {
                                for (int k = j + 1; k <= i; k++)
                                {
                                    if (j == 6 && i >= 10)
                                    {
                                        k = 10;
                                    }
                                    direction = (end.position - points[k].position).normalized;
                                    hit = Physics2D.Raycast(points[k].position, direction);

                                    if (hit.collider.gameObject.name == j.ToString())
                                    {
                                        paths[i, j] = k;
                                        break;
                                    }

                                    if (j == 6 && i >= 10)
                                    {
                                        k = j + 1;
                                    }
                                }
                            }
                            else
                            {
                                for (int k = j - 1; k >= 0; k--)
                                {
                                    direction = (end.position - points[k].position).normalized;
                                    hit = Physics2D.Raycast(points[k].position, direction);

                                    if (hit.collider.gameObject.name == j.ToString())
                                    {
                                        paths[i, j] = k;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                Debug.Log("Path " + i + ", " + j + " is " + paths[i, j]);

                if (j < 12)
                {
                    j++;
                }
                else
                {
                    j = 0;
                }
            }

            if (i < 12)
            {
                i++;
            }
            else
            {
                i = 0;
            }
        }
    }

    int FindTargetWaypoint(Vector3 target)
    {
        float dist = 0.0f;
        int i = 0;
        int waypoint = 0;
        Vector3 direction;

        foreach (Transform point in points)
        {
            if (Vector3.Distance(point.position, target) < dist || dist == 0.0f)
            {
                direction = (point.position - target).normalized;
                hit = Physics2D.Raycast(target, direction);
                if (hit.collider.gameObject.name == i.ToString())
                {
                    dist = Vector3.Distance(point.position, target);
                    waypoint = i;
                }
            }

            i++;
        }

        return waypoint;
    }

    void CreatePath()
    {
        path = new Stack<int>();
        int tempWaypoint = targetWaypoint;
        path.Push(targetWaypoint);

        do
        {
            targetWaypoint = tempWaypoint;
            tempWaypoint = paths[currentWaypoint, targetWaypoint];
            if (tempWaypoint != targetWaypoint)
            {
                path.Push(tempWaypoint);
            }
        } while (tempWaypoint != targetWaypoint);

        arrive = false;
    }
}