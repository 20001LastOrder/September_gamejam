using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMoveScript : MonoBehaviour
{
    // z from 32 to 320
    [Range(15f, 50.0f)]
    public float moveSpeedMax;
    [Range(0.0f, 10.0f)]
    public float moveSpeedMin;

    private int direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = 0;
        StartCoroutine(MoveCloud());
    }

    public IEnumerator MoveCloud()
    {
        while (true)
        {
            float speed = Random.Range(moveSpeedMin, moveSpeedMax);
            if (direction == 0)
            {
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
                if (transform.position.z > 320)
                    direction = 1;
            }
            if (direction == 1)
            {
                transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
                if (transform.position.z < 32)
                    direction = 0;
            }
            yield return new WaitForEndOfFrame();
        }

    }
}
