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
    private BoxCollider collider;
    private float groundCord;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();

        onGround = true;
        groundCord = transform.position.y;
	}

    private void Update()
    {

        // gracity constant change
        // make the player fall faster
        
        
        
        
    }

    // Update is called once per frame
    void FixedUpdate () {

        float movementH = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float movementV = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Move(movementH, movementV);

        Jump();

       

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

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded())
            {
                groundCord = transform.position.y;
                Physics.gravity = new Vector3(0, -9.98f, 0);
                // set the animator trigger to true
                anim.SetBool("onGround", false);
                anim.SetTrigger("Jump");

                rig.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
            else
            {
                Physics.gravity = new Vector3(0, Mathf.Clamp((3 / (transform.position.y - groundCord)), 0.5f, 1f) * -40, 0);
                Debug.Log(Physics.gravity + " " + (transform.position.y - groundCord));
            }

            
        }

        // start falling
        if (Input.GetKeyUp(KeyCode.Space) && !isGrounded())
        {
            //rig.velocity = new Vector3(0, -jumpForce, 0);
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
        
        //rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

        collider.enabled = false;
        Physics.Raycast(transform.position + Vector3.up * 2, Vector3.down, out hitResult, detectLength, floorLayer, QueryTriggerInteraction.Collide);
        collider.enabled = true;

        Debug.DrawLine(transform.position + Vector3.up * 2, transform.position + Vector3.down * detectLength, Color.red);

        if (hitResult.transform != null)
            Debug.Log(hitResult.transform.gameObject);

        if (Mathf.Abs(rig.velocity.y) > 0.0003 && hitResult.transform == null)
        {
            return false;
        }

        
        return true;
    }
}
