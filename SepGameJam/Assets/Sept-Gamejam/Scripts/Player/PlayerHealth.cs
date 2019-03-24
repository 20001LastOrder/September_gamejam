using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int healthPoint;
    public float bumpForce;

    private Rigidbody rig;
    private PlayerMovement moveScript;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
        moveScript = GetComponent<PlayerMovement>();
		GameFlowManager.Instance.IncreasePlayerLifeUI(healthPoint);
	}

    public void LoseHeath(int healthToLose)
    {
        healthPoint -= healthToLose;
        BumpBack();
		GameFlowManager.Instance.DecreasePlayerLifeUI(healthToLose);
		if(healthPoint <= 0) {
			GameFlowManager.Instance.GameOver();
		}
    }

    private void BumpBack()
    {
        // bump the player back with some force
        Vector3 dir = moveScript.GetBumpDirection();
        rig.AddForce(dir.normalized * bumpForce, ForceMode.Impulse);
    }
}
