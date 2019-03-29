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
        if (EnemyManager.Instance)
        {
            EnemyManager.Instance.RegisterEnemy(gameObject);
        }
	}

    public void GetHitBySpell()
    {
        // play effect
        // spawn death effect
        EnemyManager.Instance.UnregisterEnemy(gameObject);
        Instantiate(DeathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
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
