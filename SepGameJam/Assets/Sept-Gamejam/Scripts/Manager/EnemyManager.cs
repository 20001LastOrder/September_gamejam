using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : ManagerBase<EnemyManager>
{
    [SerializeField]
    private List<GameObject> enemies;

    protected override void Awake()
    {
        base.Awake();
        enemies = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            enemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public GameObject FindCloestEnemyTarget(GameObject from, float rangeW, float rangeL)
    {
        GameObject cloestTarget = null;
        float minDist = 10000;
        foreach (GameObject o in enemies)
        {
            if (Mathf.Abs(o.transform.position.x - from.transform.position.x) < rangeW)
            {
                if (Mathf.Abs(o.transform.position.z - from.transform.position.z) < rangeL){
                    // in the attack range
                    float dist = (o.transform.position - from.transform.position).sqrMagnitude;
                    if (dist < minDist) {
                        minDist = dist;
                        cloestTarget = o;
                    }
                }
            }
        }
        return cloestTarget;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
