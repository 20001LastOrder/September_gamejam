using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

    public float jumpForce;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public AudioSource playerAud;
    public AudioClip audJump;
    public AudioClip audLand;

    private bool onGround;
    private Animator anim;
    protected Rigidbody rig;
    public bool jumpRequest;
    public bool canJump = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        Physics.gravity = new Vector3(0, -13, 0);
    }

    // Use this for initialization
    void Start()
    {
        onGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded();

        // gracity constant change
        // make the player fall faster
        if (onGround)
        {
            anim.SetBool("onGround", true);
        }
        else
        {
            anim.SetBool("onGround", false);
        }

        if (Input.GetButtonDown("Jump") && onGround)
        {
            jumpRequest = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            canJump = true;
        }

    }

    private void FixedUpdate()
    {
        if (jumpRequest && canJump && onGround)
        {
            canJump = false;
            // set the animator trigger to true
            anim.SetBool("onGround", false);
            anim.SetTrigger("Jump");

            playerAud.clip = audJump;
            playerAud.Play();
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            jumpRequest = false;
        }


        if (rig.velocity.y < 0)             // falling
        {
            rig.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (!Input.GetButton("Jump") && rig.velocity.y > 0)            // jumped up but not falling
        {
            rig.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    //private bool isGrounded()
    //{
    //    RaycastHit hitResult;


    //    Physics.Raycast(transform.position + Vector3.up * 2, Vector3.down, out hitResult, detectLength, floorLayer);


    //    Debug.DrawLine(transform.position + Vector3.up * 2, transform.position + Vector3.down * detectLength, Color.red);

    //    if (hitResult.transform != null)
    //        Debug.Log(hitResult.transform.gameObject);

    //    if (Mathf.Abs(rig.velocity.y) > 0.0003 && hitResult.transform == null)
    //    {
    //        return false;
    //    }

    //    return true;
    //}

    private void isGrounded()
    {
        if (Mathf.Abs(rig.velocity.y) >= 0.003)
        {
            onGround = false;
        }
        else
        {
            onGround = true;
        }
    }

    public bool grounded()
    {
        return onGround;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Floor" && rig.velocity.y > 0.03)
    //    {
    //        playerAud.clip = audLand;
    //        playerAud.Play();
    //    }
    //}

}
