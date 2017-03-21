using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {
    Vector3 pos;
    public GameObject player; //drag player onto this field in inspector
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        pos = player.transform.position;
        pos.z = -10;
        pos.y = gameObject.transform.position.y;
        gameObject.transform.position = pos;
	}
}
