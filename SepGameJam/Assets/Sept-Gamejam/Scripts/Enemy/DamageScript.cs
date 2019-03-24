using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour {

    public GameObject DeathEffectPrefab;

    private float timer;
    private bool locker;

	// Use this for initialization
	void Start () {
        timer = 0;
	}

    public void GetHitBySpell()
    {
        // play effect

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 2);
    }

    // detect touch on health
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && locker == false)
        {

            StartCoroutine(DamagePlayer(collision.gameObject));     
            
        }
    }

    IEnumerator DamagePlayer(GameObject player)
    {
        locker = true;

        player.GetComponent<PlayerHealth>().LoseHeath(1);
        gameObject.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(3);

        gameObject.GetComponent<BoxCollider>().enabled = true;
        locker = false;
    }
    
}
