using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// store NPC events
public class NPCscript : MonoBehaviour {

    private bool inEvent = false;
    public int numLine = 0;

    private void Update()
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // some text show the player how to interact
            if (Input.GetKeyDown(KeyCode.E) && inEvent == false)
            {
                Debug.Log("trigger");
                NPCEventManager.instance.TriggerEvent("GreetPlayer", this.gameObject, NPCEventManager.EventType.instant);
                
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

    public void Talk()
    {
        Debug.Log("execute");
        inEvent = true;
        DialogueManager.instance.Show(numLine);
    }

    public void ZoomoutCamera()
    {
        Debug.Log("zoom out the camera");
    }
    
}
