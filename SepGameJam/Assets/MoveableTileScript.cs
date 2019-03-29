using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableTileScript : MonoBehaviour
{
    public Vector3 pointE;
    public Vector3 pointS;
    public float waitTime;
    public float speed;

    private Vector3 dest;

    private void Start()
    {
        StartCoroutine(MoveTile());
    }

    public IEnumerator MoveTile()
    {
        Vector3 sum = pointE + pointS;
        if (transform.position != pointE || transform.position != pointS)
        {
            transform.position = pointS;
            dest = pointE;
        }
        while (true)
        {
            if (Mathf.Abs(dest.z - transform.position.z) <= 0.1)
            {
                dest = sum - dest;
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
