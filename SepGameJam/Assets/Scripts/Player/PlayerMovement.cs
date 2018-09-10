using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 2;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Move();

	}

    private void Move()
    {
        float movementH = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float movementV = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 newPosition = transform.position + new Vector3(movementH, 0, movementV);
        transform.position = newPosition; 
    }
}
