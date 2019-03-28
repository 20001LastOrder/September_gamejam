using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NPCType
{
    Demon,
    Villiager
}

// store NPC events
public class NPCscript : MonoBehaviour {

    [SerializeField]
    private bool inEvent = false;
    [SerializeField]
    private NPCType role;

    public int stage = 0;

    public Text interactText;

    private void Start()
    {
        transform.position = new Vector3(18.91f, 11.25f, 44.81f);
        if (GameFlowManager.Instance)
        {
            GameFlowManager.Instance.RegisterNpc(NPCType.Demon, GetComponent<NPCscript>());
            Debug.Log("successfully register demon to GameManager");
        }
    }

    private void Update()
    {
        if (DialogueManager.instance != null)
        {
            if (Input.GetKeyDown(KeyCode.E) && inEvent == true)
            {
                if (interactText.enabled) interactText.enabled = false;
                DialogueManager.instance.nextLine = true;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // some text show the player how to interact
            interactText.enabled = true;
            if (inEvent == false)
            {
                NPCEventManager.instance.TriggerEvent(stage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactText.enabled = false;
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
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        Transform cam = camera.transform;
        Debug.Log("zoom in the camera");
        Vector3 dir = transform.forward;
        StartCoroutine(ShiftCamera(cam.position, transform.position + dir * 25 + new Vector3(0, 7.5f, 0), cam, true));
    }

    public void Talk(int numLine)
    {
        inEvent = true;
        DialogueManager.instance.Show(numLine);
    }

    public void ZoomoutCamera()
    {
        Debug.Log("zoom out the camera");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        Transform cam = camera.transform;
        Vector3 dest = CameraMovement.instance.GetCameraFollowPosition();
        StartCoroutine(ShiftCamera(cam.position, dest, cam, false));
    }

    public IEnumerator MoveTo(Vector3 pos)
    {
        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, 0.2f);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ShiftCamera(Vector3 from, Vector3 to, Transform cam, bool zoomIn)
    {
        if (zoomIn) CameraMovement.instance.inEvent = true;
        while ((from - to).sqrMagnitude >= 0.3f)
        {
            cam.position = Vector3.MoveTowards(from, to, 0.5f);
            from = cam.transform.position;
            yield return new WaitForEndOfFrame();
        }
        if (!zoomIn) CameraMovement.instance.inEvent = false; 
    }

}
