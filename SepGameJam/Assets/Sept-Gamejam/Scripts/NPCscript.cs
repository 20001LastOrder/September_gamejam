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
    public Vector3 secondStagePosition;

    public Text interactText;
	public float shiftSpeed = 1.2f;
	public int triggerSide = 1; 	//determine if he will talk to player during the day or night

    private GameObject player;

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
                //DialogueManager.instance.nextLine = true;
            }
        }

        // move the npc to the next stage
        if (GameObject.Find("Player1") != null && stage == 0)
        {
            if (GameObject.Find("Player1").transform.position.z > 110)
            {
                transform.position = secondStagePosition;
                stage = 1;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.tag == "Player" && GameFlowManager.Instance.GetSide() == triggerSide)
        {
            if (!player) player = other.gameObject;
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
		if (other.gameObject.tag == "Player" && GameFlowManager.Instance.GetSide() == triggerSide)
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

    public void ZoominCamera(int facing)
    {
        //if (CameraMovement.instance.side != facing)
        //{
        //    StartCoroutine(CameraMovement.instance.RotateCamera());
        //}
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        Transform cam = camera.transform;
        //Debug.Log("zoom in the camera");
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
        //Debug.Log("zoom out the camera");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		Transform cam = camera.transform;
		Vector3 start = cam.position;
        Vector3 dest = CameraMovement.instance.GetCameraFollowPosition();
        StartCoroutine(ShiftCamera(start, dest, cam, false));
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
        yield return new WaitUntil(() => CameraMovement.instance.inRotation == false);

        if (zoomIn) CameraMovement.instance.inEvent = true;
        while ((from - to).sqrMagnitude >= 0.1f)
        {
            cam.position = Vector3.MoveTowards(from, to, shiftSpeed);
            from = cam.transform.position;
            cam.LookAt(player.transform);
            yield return new WaitForEndOfFrame();
        }
        cam.transform.position = to;
        if (!zoomIn) CameraMovement.instance.inEvent = false; 
    }

}
