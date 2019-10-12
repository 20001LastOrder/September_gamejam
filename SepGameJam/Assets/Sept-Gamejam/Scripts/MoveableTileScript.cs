using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MoveableTileScript : MonoBehaviour
{
    public Vector3 pointE;
    public Vector3 pointS;
    public float waitTime;
    public float speed;

    private Vector3 dest;
    private Vector3 movingDistance;
    private float timer;
    private Vector3 sum;

    private List<Transform> carryings;

    private void Start()
    {
        carryings = new List<Transform>();
        movingDistance = Vector3.zero;

        sum = pointE + pointS;
        if (transform.position != pointE || transform.position != pointS)
        {
            transform.position = pointS;
            dest = pointE;
        }
    }

    private void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (Mathf.Abs(dest.z - transform.position.z) <= 0.1)
            {
                movingDistance = Vector3.zero;
                dest = sum - dest;
                timer = waitTime;
            }
            else
            {
                Vector3 lastPos = transform.position;
                transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
                
                foreach (Transform t in carryings)
                {
                    t.position += (transform.position - lastPos);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            carryings.Add(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            carryings.Remove(other.gameObject.transform);
        }
    }
}
