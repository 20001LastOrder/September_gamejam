﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public static CameraMovement instance = null;

    public float cameraDistance;
    public float verticalHeight;
    public float turnRate;
    public float followSpeed;
    public float followDamping;
    public float timeRate;
    public GameObject target;
    [HideInInspector]
    public bool inRotation = false;
    [HideInInspector]
    public bool switchStateOn = false;
    public bool inEvent = false;
	public int side = 0;	//right(bright) side = 0, left(dark) side = 1

	[Space]
	public Material skybox_day;
	public Material skybox_night;
	private bool changeSkybox;

    private Vector3 direction;
    private Transform spellFollowTarget;
    private Transform lookAtTarget;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        //ResetCameraPosition();
       // StartCoroutine(FollowCamera());
    }

	private void OnEnable() {
        //Restart corotine when the camera is activated(go back from pause menu)
        side = 0;
		StartCoroutine(FollowCamera());
	}


	// Update is called once per frame
	private void Update()
    {
        // if some key is pressed then switch the camera position to the other side
        if (inRotation == false && inEvent == false)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                switchStateOn = !switchStateOn;
                cameraDistance *= -1;
           
                StartCoroutine(RotateCamera());
            }
        }

    }

    private void ResetCameraPosition()
    {
        if (target != null)
        {
            Vector3 position = target.transform.position + new Vector3(cameraDistance, verticalHeight, 0);
            
            transform.position = position;

            transform.LookAt(target.transform);
        }
    }

    private IEnumerator FollowCamera()
    {
        while (true)
        {
            // if is not rotating around the player the camera needs to follow the player
            if (inRotation == false && inEvent == false)
            {
                // get a location beside the player that the camera need to be
                Vector3 newPosition = GetCameraFollowPosition();

                // the cmaera follow is speed is determined by the distance to the location
                Vector3 direction = newPosition - transform.position;
                followSpeed = direction.magnitude / timeRate;

                // move towards the location
                transform.position = Vector3.MoveTowards(transform.position, newPosition, followSpeed);
                transform.LookAt(lookAtTarget);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public Vector3 GetCameraFollowPosition()
    {
        int mul = side == 0 ? 1 : -1;
            lookAtTarget = target.transform;
            return target.transform.position + new Vector3(cameraDistance, verticalHeight, 0);  
    }

    public IEnumerator RotateCamera()
    {
        inRotation = true;
        float rotateAngle = 0;

        while (rotateAngle < 180)
        {
            //Debug.Log(Mathf.Sin(Mathf.Clamp(Time.time % Mathf.PI, 0, Mathf.PI)));
            direction = target.transform.position - transform.position;

            //float tweaker = Mathf.Sin(Mathf.Clamp(Time.time % Mathf.PI, 0, Mathf.PI)) * 2;
            float angle = turnRate * Time.deltaTime;
            Quaternion desiredRotation = Quaternion.Euler(0, angle, 0); // create an angle as the mouse moves

            transform.position = target.transform.position - (desiredRotation * direction); // make the camera always keep some distance with the target as it rotates

            transform.LookAt(target.transform); // the camera needs to look at the target in order to have it rotate around it

            rotateAngle += angle;
            rotateAngle = Mathf.Clamp(rotateAngle, 0, 180);

			// change skybox 
			if (rotateAngle > 135 && !changeSkybox) {
				changeSkybox = true;
				int s = 1 - side;
				switch (s) {
				case 0:
					GameFlowManager.Instance.SwitchToDay ();
					RenderSettings.skybox = skybox_day;
					break;
				case 1:
					GameFlowManager.Instance.SwitchToNight ();
					RenderSettings.skybox = skybox_night;
					break;
				}
			}

            yield return new WaitForEndOfFrame();
        }
        side = 1 - side;
        inRotation = false;
		changeSkybox = false;
    }

    public void SpellCameraFollow(Transform target)
    {
        spellFollowTarget = target;
    }
}
