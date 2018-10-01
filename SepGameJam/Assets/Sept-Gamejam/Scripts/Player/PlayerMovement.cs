using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    public float speed = 2;
    
    
    private Animator anim;
    protected Rigidbody rig;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        

        Physics.gravity = new Vector3(0, -20, 0);
    }


    private void Update()
    {
        anim.SetFloat("Velocity", rig.velocity.y);
    }

    // Update is called once per frame
    void FixedUpdate () {

        float movementH = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float movementV = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Move(movementH, movementV);

        AnimateMovement(movementH, movementV);
    }

    private void Move(float h, float v)
    {
        // Not allowed to move during the camera turn
        if (CameraMovement.instance.inRotation == true)
        {
            return;
        }

        // During the movement, if it's on the left side of the map we need to swtich the controls
        // which means left == right
        if (CameraMovement.instance.switchStateOn == false)
        {
            Vector3 newPosition = transform.position + new Vector3(-v, 0, h);
            transform.position = newPosition;
            Turn(h, -v);
        }
        else
        {
            Vector3 newPosition = transform.position + new Vector3(v, 0, -h);
            transform.position = newPosition;
            Turn(-h, v);
        }
    }

    

    private void Turn(float h, float v)
    {
        
            // go forward
            if (h > 0)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward);
                transform.rotation = rotation;
            }

            // go backward
            if (h < 0)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.back);
                transform.rotation = rotation;
            }

            // go left
            if (v < 0)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.left);
                transform.rotation = rotation;
            }

            // go right
            if (v > 0)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.right);
                transform.rotation = rotation;
            }
        
    }

    private void AnimateMovement(float h, float v)
    {
        if (h != 0 || v != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        onGround = true;
    //        Debug.Log("touching");
    //        anim.SetBool("onGround", true);
    //    }
    //}

   

    public Vector3 GetBumpDirection()
    {
        return -transform.forward;
    }
}
