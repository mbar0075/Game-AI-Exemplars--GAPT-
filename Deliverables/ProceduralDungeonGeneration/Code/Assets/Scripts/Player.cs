using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.5f;
    public Rigidbody2D rb;

    private Vector3 movement;
    private Vector3 dest;
    private Vector3 p;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        dest = transform.position + movement;
        p = Vector3.MoveTowards(transform.position, dest, speed);
        rb.MovePosition(p);
    }
}
