using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// store NPC events
public class NPCscript : MonoBehaviour {

    private bool inEvent = false;
    public int stage = 0;

    private void Start()
    {
        transform.position = new Vector3(18.91f, 11.25f, 44.81f);
    }

    private void Update()
    {
        if (DialogueManager.instance != null)
        {
            if (Input.GetKeyDown(KeyCode.E) && inEvent == true)
            {
                DialogueManager.instance.nextLine = true;
            }
            else
            {
                DialogueManager.instance.nextLine = false;
            }
        }

        // move the npc to the next stage
        if (GameObject.Find("Player1") != null && stage == 0)
        {
            if (GameObject.Find("Player1").transform.position.z > 110)
            {
                transform.position = new Vector3(20, 5.61f, 355);
                stage = 1;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // some text show the player how to interact
            if (Input.GetKeyDown(KeyCode.E) && inEvent == false)
            {
                
                NPCEventManager.instance.TriggerEvent(stage);
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EndState();
        }
    }

    public void EndState()
    {
        inEvent = false;
        DialogueManager.instance.SetActive(false);
        NPCEventManager.instance.Reset();
    }

    public void ZoominCamera()
    {
        Debug.Log("zoom in the camera");
    }

    public void Talk(int numLine)
    {
        inEvent = true;
        DialogueManager.instance.Show(numLine);
    }

    public void ZoomoutCamera()
    {
        Debug.Log("zoom out the camera");
        
    }
    
}
