using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetecter : MonoBehaviour {

    public Transform player = null;
    public Vector3 checkPoint;
    public float height;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {

            player.position = checkPoint + new Vector3(0f, height, 0f);
        }
    }
}
