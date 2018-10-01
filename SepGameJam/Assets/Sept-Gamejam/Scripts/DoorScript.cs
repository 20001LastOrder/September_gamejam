using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public Vector3 destination;
    public TextMesh doorText;

    [SerializeField]
    private Color _color;

    public bool locked;
    private GameObject entity;
    private MeshRenderer textRenderer;
    

    private void Start()
    {
        var materials = gameObject.GetComponentsInChildren<MeshRenderer>();
        for (int i=0; i < materials.Length; i++)
        {
            materials[i].material.SetColor("_EmissionColor", _color);
        }

        textRenderer = transform.GetChild(2).gameObject.GetComponent<MeshRenderer>();
        textRenderer.enabled = false;
        
        if (doorText == null)
        {
            Debug.LogError("No 3d textmesh reference");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && !locked)
        {
            if (entity != null)
            {
                entity.transform.position = destination;
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
        textRenderer.enabled = false;
    }

    public void Unlock()
    {
        locked = false;
    }
}
