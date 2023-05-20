using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : MonoBehaviour {
    //Declaring variables and game objects to be used
    [SerializeField] private int hp = 100;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public GameObject canvasObject;

    //Display current tower health with GUI
    GUIStyle guiStyle = new GUIStyle();
    void OnGUI() {
        guiStyle.fontSize = 22;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup (new Rect (10, 10, 450, 450));
        GUI.Box (new Rect (10,100,450,450), "Tower Health: " + hp, guiStyle);
        GUI.EndGroup ();
    }
    
    //Function to make the canvas active
    private void MakeActive() {
        canvasObject.SetActive(true);
    }

    //On collision wiith game object of tag Enemy
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            //Decrease hp by 5 and change color to be slightly more red
            hp -= 5;
            spriteRenderer.color = spriteRenderer.color + new Color (0f, -0.05f, -0.05f, 0f);
            //Destroy the enemy game object
            Destroy(collision.gameObject);
            //If hp is zero, call the make active function
            if (hp == 0) {
                MakeActive();
            }
        }
    }
}
