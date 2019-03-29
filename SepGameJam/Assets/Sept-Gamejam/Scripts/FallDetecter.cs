using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetecter : MonoBehaviour {

    public Transform player = null;
    public Vector3 checkPoint;
    public float height;


    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
			var health = player.GetComponent<PlayerHealth>();
			if(health!= null) {
				health.LoseHeath(1);
			}
			//Application.OpenURL("www.google.com");
            player.position = checkPoint + new Vector3(0f, height, 0f);
            player.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}
