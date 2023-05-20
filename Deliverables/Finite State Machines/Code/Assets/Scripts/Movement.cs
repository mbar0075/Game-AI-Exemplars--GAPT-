using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float speed = 0.4f;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector3 movement;
    private Vector3 dest;
    private Vector3 p;
    private Ghost ghost;

    private GameObject[] pellets;
    private GameObject[] powerPellets;
    private bool allDestroyed = true;

    void Update()
    {
        allDestroyed = true;
        pellets = GameObject.FindGameObjectsWithTag("Pellet");
        powerPellets = GameObject.FindGameObjectsWithTag("Power");

        foreach (GameObject pellet in pellets)
        {
            if (pellet.activeSelf)
            {
                allDestroyed = false;
                break;
            }
        }

        foreach (GameObject pellet in powerPellets)
        {
            if (pellet.activeSelf)
            {
                allDestroyed = false;
                break;
            }
        }

        if (allDestroyed)
        {
            SceneManager.LoadScene("Winner");
        }
    }

    void FixedUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("DirX", movement.x);
        animator.SetFloat("DirY", movement.y);

        dest = transform.position + movement;
        p = Vector3.MoveTowards(transform.position, dest, speed);
        rb.MovePosition(p);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ghost"))
        {
            ghost = col.gameObject.GetComponent<Ghost>();
            if (ghost.state != "Frightened")
            {
                Destroy(gameObject);
                SceneManager.LoadScene("GameOver");
            } else
            {
                ghost.marker.transform.position = new Vector3(14.5f, 20.0f, 0.0f);
                ghost.transform.position = new Vector3(14.0f, 17.0f, 0.0f);
                ghost.state = "Sleep";
                ghost.elapsed = 0;
                ghost.arrived = true;
                ghost.sr.sprite = ghost.normal1;
                ghost.transform.localScale = new Vector3(0.06f, 0.06f, 0.0f);
                ghost.ani.enabled = true;
            }
        }
    }
}
