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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<DamageScript>().GetHitBySpell();
        }

        // explosion effect

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 2f);
    }
}
