using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour {
    Rigidbody2D birdRB;
    public float upspeed = 0.001f;
    public float forewardspeed = 0.001f;
    Vector3 up = new Vector3(0, 1, 0);
    Vector3 right = new Vector3(1, 0, 0);
    Vector3 zero = new Vector3(0, 0, 0);
    bool start;
    bool dead;
	// Use this for initialization
	void Start () {
        start = false;
        dead = false;
        birdRB = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!dead)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
            {
                birdRB.velocity = up * upspeed + right * forewardspeed;
                if (!start)
                {
                    birdRB.gravityScale = 0.7f;
                    start = true;
                }
            }
        }
        else
        {
            birdRB.velocity = zero;
        }
        
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collide with " + collision.gameObject.name);
        if (collision.gameObject.name == "floor"
            || collision.gameObject.name == "pipe")
        {
            birdRB.velocity = zero;
            dead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("triggered " + collision.gameObject.name);
        if (collision.gameObject.name == "background" 
            || collision.gameObject.name == "floor"
            || collision.gameObject.name == "pipes")
        {
            Vector3 pos = collision.gameObject.transform.position;
            
            if (collision.gameObject.name == "pipes" || collision.gameObject.name == "floor" )
            {
                if(collision.gameObject.name == "pipes")
                {
                    float y = Random.Range(-0.7f, 0.7f);
                    pos.y = y;
                }
                pos.x += 4 * 1.674f; 
            }
            else
            {
                pos.x += 4 * 1.42f;
            }

            collision.gameObject.transform.position = pos;

        }


    }
}
