using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    //bullet travel speed
    public float speed = 5;

    //bullet lifespan
    public float lifespan = 3;
    void Start()
    {
        //Destroying the bullet after the assigned time
        Invoke("RemoveObj", lifespan);
    }

    void Update()
    {
        //Moving the bullet upwards at a constant speed
        this.transform.position += this.transform.up * speed * Time.deltaTime;
    }

    void RemoveObj()
    {
        //Destroying the bullet object
        Destroy(this.gameObject);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //If the bullet hits an invader, said inavder is deactivated and the bullet is removed
        if (other.tag == "Invader")
        {
            other.gameObject.SetActive(false);
            RemoveObj();
        }
    }
}
