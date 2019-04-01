using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureScript : MonoBehaviour {

    GameObject owner;
    [Header("Follow setting")]
    public float followDistance = 5f;
    public float followVertical = 5f;
    [Space]
    [Header("Attack setting")]
    [SerializeField]
    private float shootingRangeWidth;
    [SerializeField]
    private float shootingRangeLength;
    [SerializeField]
    private float shootingForce;
    [SerializeField]
    private bool isFiring = false;
    [SerializeField]
    private float timer = 0;
    private float lastCastTime = 0;
    [Space]
    [SerializeField]
    private List<string> simple_SpellList;
    [SerializeField]
    private List<string> hard_SpellList;
    [SerializeField]
    private List<string> safe_SpellList;
    [Space]
    public GameObject SpellPrefab;

    private Queue<string> cur_spell;

    public Color baseColor;
    private Color tempColor;

    private void Awake()
    {
        cur_spell = new Queue<string>();
        gameObject.GetComponent<MeshRenderer>().material.color = baseColor;
        tempColor = gameObject.GetComponent<MeshRenderer>().material.color;
    }


    private void Update()
    {
        if (owner)
        {
            if (Input.GetKey(KeyCode.F) && !isFiring)
            {
                Debug.Log("press F");
                // record how long the key is held
                timer += Time.deltaTime;
                tempColor = Color.LerpUnclamped(tempColor, Color.red, Time.deltaTime / 5);
                gameObject.GetComponent<MeshRenderer>().material.color = tempColor;
            }
            else if (Input.GetKeyUp(KeyCode.F) && timer > 0 && !isFiring)
            {
                isFiring = true;
                
                // generate a spell that for player to press
                cur_spell = GenerateSpell(timer);
                //StartCoroutine(PrepareSpell(cur_spell));

                lastCastTime = Time.time;
            }

            if (isFiring && Time.time - lastCastTime > 10)
            {
                GameFlowManager.Instance.UIResetSpellKeys();
                timer = 0;
                isFiring = false;
            }

            if (cur_spell != null && cur_spell.Count > 0)
            {
                //lerp back to the base color
                tempColor = Color.LerpUnclamped(tempColor, baseColor, 0.01f);
                gameObject.GetComponent<MeshRenderer>().material.color = tempColor;

                string input = Input.inputString;
                if (!safe_SpellList.Contains(input) && input != "")
                {
                    Debug.Log(input);
                    //check if it is the right one
                    if (input == cur_spell.Peek())
                    {
                        // correct
                        Debug.Log("correctly press " + input);
                        cur_spell.Dequeue();
                        // get rid of it on canvas
                        GameFlowManager.Instance.UIRemoveSpellKey();
                        // if all are correct
                        if (cur_spell.Count == 0)
                        {
                            // cast the spell successfully
                            Debug.Log("cast successfully");
                            CastSpell();
                            timer = 0;
                            isFiring = false;
                        }
                    }
                    else
                    {
                        // fail, break
                        Debug.Log("wrong input " + input);
                        GameFlowManager.Instance.UIResetSpellKeys();
                        timer = 0;
                        isFiring = false;
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (owner)
        {
            Vector3 dir = -owner.transform.forward;
            Vector3 pos = owner.transform.position + followDistance * dir + new Vector3(0, followVertical, 0);
            float delta = (pos - transform.position).sqrMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, pos, delta);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //GameFlowManager.Instance.ObtainCube();
            SetOwner(other.gameObject);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void SetOwner(GameObject obj)
    {
        owner = obj;
    }

    private Queue<string> GenerateSpell(float time)
    {
        Queue<string> spell = new Queue<string>();
        List<string> spellKeys = new List<string>();
        int level = (int)(time / 2) % 3;
        if (time < 1) level = 0;
        Debug.Log(level);
        switch (level)
        {
            case 0:
                // 3 keys in simple list
                for (int i=0; i<3; i++)
                {
                    int ran = Random.Range(0, simple_SpellList.Count);
                    spell.Enqueue(simple_SpellList[ran]);
                    spellKeys.Add(simple_SpellList[ran]);
                }
                break;
            case 1:
                // 4 keys in simple list
                for (int i = 0; i < 4; i++)
                {
                    int ran = Random.Range(0, simple_SpellList.Count);
                    spell.Enqueue(simple_SpellList[ran]);
                    spellKeys.Add(simple_SpellList[ran]);
                }
                break;
            case 2:
                // 5 keys in hard list
                for (int i = 0; i < 4; i++)
                {
                    int ran = Random.Range(0, hard_SpellList.Count);
                    spell.Enqueue(hard_SpellList[ran]);
                    spellKeys.Add(hard_SpellList[ran]);
                }
                break;
            default:
                break;
        }
        GameFlowManager.Instance.UICreateSpellKeys(spellKeys);
        return spell;
    }

    private void CastSpell()
    {
        // get the closest target
        GameObject target = EnemyManager.Instance.FindCloestEnemyTarget(gameObject, shootingRangeWidth, shootingRangeLength);
        
        // shoot the spell (fireball or beam or something)
        Vector3 targetDir;
        if (target == null)
        {
            targetDir = transform.forward;
        }
        else
        {
            targetDir = (target.transform.position + new Vector3(0, 3, 0) - transform.position).normalized;
            Debug.Log(target);
        }

        GameObject fireBall = Instantiate(SpellPrefab, transform.position + targetDir * 0.2f, Quaternion.identity);
        fireBall.GetComponent<CubeFireBall>().SetFirePower(timer);
        shootingForce = 30 + timer * 7;
        fireBall.GetComponent<Rigidbody>().AddForce(targetDir * shootingForce);
    }
}
