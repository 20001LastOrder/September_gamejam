using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour {

    public GameObject player;

    private PlayerHealth healthScript;
    private float timer;
    private bool locker;

	// Use this for initialization
	void Start () {
        healthScript = player.GetComponent<PlayerHealth>();
        timer = 0;
	}

    void Update()
    {
        
    }

    // detect touch on health
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && locker == false)
        {

            StartCoroutine(DamagePlayer());     
            
        }
    }

    IEnumerator DamagePlayer()
    {
        locker = true;

        healthScript.LoseHeath(1);

        yield return new WaitForSeconds(3);

        locker = false;
    }
    
}
