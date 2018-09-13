using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 2;
    public float jumpForce;
    public float detectLength;
    public LayerMask floorLayer;

    private bool onGround;
    private Animator anim;
    private Rigidbody rig;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        onGround = true;
	}

    private void Update()
    {
        Jump();

        if (rig.velocity.y < 0)
        {
            Physics.gravity = new Vector3(0, -40f, 0);
        }
        else
        {
            Physics.gravity = new Vector3(0, -9.98f, 0);
        }
    }

    // Update is called once per frame
    void FixedUpdate () {

        float movementH = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float movementV = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Move(movementH, movementV);

        Turn(movementH, movementV);

        AnimateMovement(movementH, movementV);
    }

    private void Move(float h, float v)
    {
        Vector3 newPosition = transform.position + new Vector3(v, 0, h);
        transform.position = newPosition; 
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Physics.gravity = new Vector3(0, -9.98f, 0);
            // set the animator trigger to true
            anim.SetBool("onGround", false);
            anim.SetTrigger("Jump");
            
        }

        if (isGrounded())
        {
            anim.SetBool("onGround", true);
        }
        else
        {
            anim.SetBool("onGround", false);
        }
    }

    public void Raise()
    {
        rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

    private bool isGrounded()
    {
        RaycastHit hitResult;

        Physics.Raycast(transform.position, Vector3.down, out hitResult, detectLength, floorLayer, QueryTriggerInteraction.Ignore);

        if (hitResult.transform != null)
            Debug.Log(hitResult.transform.gameObject);

        if (Mathf.Abs(rig.velocity.y) > 0.005 && hitResult.transform == null)
        {
            return false;
        }
        return true;
    }
}
