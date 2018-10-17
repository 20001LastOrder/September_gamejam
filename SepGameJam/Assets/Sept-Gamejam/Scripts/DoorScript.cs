using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour {

    public Vector3 destination;
    public TextMesh doorText;

    public int goldReq = 10;
    public int rubyReq = 5;
    public GameObject IwantKeyButton;

    [SerializeField]
    private Color _color;

    [SerializeField]
    private bool lastDoor;

    public bool locked;
    private GameObject entity;
    private MeshRenderer textRenderer;
    

    private void Start()
    {
        var materials = gameObject.GetComponentsInChildren<MeshRenderer>();
        for (int i=0; i < materials.Length; i++)
        {
            materials[i].material.SetColor("_EmissionColor", _color * 2.2f);
            
        }

        textRenderer = transform.GetChild(2).gameObject.GetComponent<MeshRenderer>();
        textRenderer.enabled = false;
        
        if (doorText == null)
        {
            Debug.LogError("No 3d textmesh reference");
        }

        IwantKeyButton = GameObject.Find("I want the key now");
    }

    private void OnTriggerStay(Collider other)
    {
        if (lastDoor)
        {
            // win condition
            if (GameFlowManager.Instance.getKey())
            {
                locked = false;
            }
            else if (GameFlowManager.Instance.getGold() >= goldReq && GameFlowManager.Instance.getRuby() >= rubyReq)
            {
                locked = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !locked)
        {
            if (entity != null && !lastDoor)
            {
                entity.transform.position = destination;
            }
            else
            {
                // win condition
                GameFlowManager.Instance.Win();
            }
        }

        
    }

    // show the text and enable the function
    private void OnTriggerEnter(Collider other)
    {
        entity = other.gameObject;

        if (other.gameObject.tag == "Player" && locked)
        {
            textRenderer.enabled = true;
            IwantKeyButton.GetComponent<Text>().enabled = true;
            doorText.text = "A key is required";
        }
        else if (other.gameObject.tag == "Player")
        {
            textRenderer.enabled = true;
            doorText.text = "Press E to enter";
        }
        
    }

    // reset the everything
    private void OnTriggerExit(Collider other)
    {
        IwantKeyButton.GetComponent<Text>().enabled = false;
        textRenderer.enabled = false;
    }

    public void Unlock()
    {
        locked = false;
    }
}
