using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Ghost : MonoBehaviour
{
    public AIDestinationSetter dest;
    public GameObject target;
    public GameObject marker;
    public Transform[] waypoints;
    public Rigidbody2D rb;
    public Animator animator;
    public float duration;
    public string state;
    public Sprite normal1;
    public Sprite normal2;
    public Sprite normal3;
    public Sprite normal4;
    public Sprite frightened;
    public Vector3 initialPos;
    public Vector3 markerPos;
    public float elapsed = 0.0f;
    public bool arrived;
    public SpriteRenderer sr;
    public Animator ani;

    private int cur = 0;
    private Vector3 p1;
    private Vector3 p2;
    private float sphereRadius = 1.0f;
    private GameObject random;
    private Collider[] colliders;
    private bool check = false;

    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        random = new GameObject("Rand");
        random.AddComponent<CircleCollider2D>();
        random.tag = "FrightNode";
        (random.GetComponent<CircleCollider2D>()).isTrigger = true;
        arrived = true;
        initialPos = transform.position;
        markerPos = marker.transform.position;

        if (gameObject.name == "Blinky")
        {
            state = "Scatter";
        } else if (gameObject.name == "Pinky")
        {
            state = "Sleep";
        }
        else if (gameObject.name == "Inky")
        {
            state = "Sleep";
        }
        else if (gameObject.name == "Clyde")
        {
            state = "Sleep";
        }
    }

    void Update()
    {
        switch (state)
        {
            case "Chase":
                Chase();
                break;
            case "Scatter":
                Scatter();
                break;
            case "Sleep":
                Sleep();
                break;
            case "Frightened":
                Frightened();
                break;
        }
    }

    void Chase()
    {
        dest.target = target.transform;

        Vector2 dir = (Vector2)dest.target.transform.position - (Vector2)transform.position;
        animator.SetFloat("DirX", dir.x);
        animator.SetFloat("DirY", dir.y);

        if (elapsed < 15.0f)
        {
            elapsed += Time.deltaTime;
        }
        else
        {
            state = "Scatter";
            elapsed = 0;
        }
    }

    void Scatter()
    {
        dest.target = waypoints[cur];

        Vector2 dir = waypoints[cur].position - transform.position;
        animator.SetFloat("DirX", dir.x);
        animator.SetFloat("DirY", dir.y);

        if (elapsed < 10.0f)
        {
            elapsed += Time.deltaTime;
        }
        else
        {
            state = "Chase";
            elapsed = 0;
        }
    }

    void Sleep()
    {
        dest.target = marker.transform;

        if (elapsed < duration)
        {
            elapsed += Time.deltaTime;
        } else
        {
            transform.position = marker.transform.position;
        }
    }

    void Frightened()
    {
        if (sr.sprite == normal1 || sr.sprite == normal2 || sr.sprite == normal3 || sr.sprite == normal4)
        {
            ani.enabled = false;
            sr.sprite = frightened;
            transform.localScale = new Vector3(0.01f, 0.01f, 0.0f);
        }

        if (arrived)
        {
            do
            {
                random.transform.position = new Vector3(Random.Range(2.0f, 27.0f), Random.Range(2.0f, 29.5f), 0.0f);
                colliders = Physics.OverlapSphere(random.transform.position, sphereRadius);

                if (colliders.Length > 0)
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
            } while (check == true);

            arrived = false;
            dest.target = random.transform;

            Vector2 dir = (Vector2)dest.target.transform.position - (Vector2)transform.position;
            animator.SetFloat("DirX", dir.x);
            animator.SetFloat("DirY", dir.y);
        }

        if (elapsed < 15.0f)
        {
            elapsed += Time.deltaTime;
        }
        else
        {
            state = "Scatter";
            elapsed = 0;
            arrived = true;
            sr.sprite = normal1;
            transform.localScale = new Vector3(0.06f, 0.06f, 0.0f);
            ani.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Node"))
        {
            if (cur == waypoints.Length-1)
            {
                cur = 0;
            } else
            {
                cur++;
            }
        } else if (state == "Sleep" && col.gameObject.name == "Start")
        {
            state = "Scatter";
            elapsed = 0.0f;
        } else if (state == "Frightened" && col.gameObject.CompareTag("FrightNode"))
        {
            arrived = true;
        }
    }
}