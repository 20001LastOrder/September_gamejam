using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlatformerEnemyMovement : MonoBehaviour {

    [System.Serializable]
    public struct PathPoint
    {
        public Vector3 point;
        public bool isVisited;
    };

    public Transform player;
    public List<PathPoint> path;
    public bool patrolOn;
    public float moveSpeed;
    public float activateDistance;

    private Animator anim;
    public int pathIndex = 0;
    private bool onWard = true;

    // Use this for initialization
    void Start () {
        patrolOn = true;

        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        Patrol();

	}

    private void Patrol()
    {
        if (patrolOn == true)
        {
            anim.SetBool("isPatrolling", true);
            // get the cloest point
            double minDistance = double.MaxValue;
            PathPoint cloestPoint;
            for (int i=0; i < path.Count; i++)
            {
                double dis = Vector3.SqrMagnitude(path[i].point - transform.position);
                if (dis < minDistance)
                {
                    minDistance = dis;
                    cloestPoint = path[i];
                }
            }

            // patrol along the path          
            // if the player is close enough          
            //Vector3.Magnitude(transform.position - player.position) < activateDistance
            if (Vector3.Magnitude(transform.position - path[pathIndex].point) < 0.1
                ) {
                if (pathIndex == path.Count - 1) onWard = false;
                if (pathIndex == 0) onWard = true;

                if (onWard) pathIndex++;
                else pathIndex--;
                
            }

            transform.position = Vector3.MoveTowards(transform.position, path[pathIndex].point, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(path[pathIndex].point - transform.position);
            
        }
    }
}
