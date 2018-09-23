﻿using System;
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
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoseHeath(int healthToLose)
    {
        healthPoint -= healthToLose;

        BumpBack();
    }

    private void BumpBack()
    {
        // bump the player back with some force
        Vector3 dir = moveScript.GetBumpDirection();
        rig.AddForce(dir * bumpForce, ForceMode.Impulse);
    }
}