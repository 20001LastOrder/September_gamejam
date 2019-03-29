using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFireBall : MonoBehaviour
{
    public GameObject explosionEffectPrefab;
    private float firePower;

    public void SetFirePower(float Powerlevel)
    {
        firePower = Powerlevel;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") return;

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<DamageScript>().GetHitBySpell();
        }

        // explosion effect

        gameObject.GetComponent<PKFxFX>().enabled = false;
        Destroy(gameObject, 2f);
    }

    
}
