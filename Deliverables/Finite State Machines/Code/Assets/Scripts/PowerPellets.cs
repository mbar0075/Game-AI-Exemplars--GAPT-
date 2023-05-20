using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellets : MonoBehaviour
{
    private Ghost blinky;
    private Ghost pinky;
    private Ghost inky;
    private Ghost clyde;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Pacman")
        {
            Destroy(gameObject);

            blinky = GameObject.Find("Blinky").GetComponent<Ghost>();
            pinky = GameObject.Find("Pinky").GetComponent<Ghost>();
            inky = GameObject.Find("Inky").GetComponent<Ghost>();
            clyde  = GameObject.Find("Clyde").GetComponent<Ghost>();

            if (blinky.state != "Sleep")
            {
                blinky.state = "Frightened";
                blinky.elapsed = 0;
            }

            if (pinky.state != "Sleep")
            {
                pinky.state = "Frightened";
                pinky.elapsed = 0;
            }

            if (inky.state != "Sleep")
            {
                inky.state = "Frightened";
                inky.elapsed = 0;
            }

            if (clyde.state != "Sleep")
            {
                clyde.state = "Frightened";
                clyde.elapsed = 0;
            }
        }
    }
}
