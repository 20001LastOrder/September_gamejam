using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCEventManager : MonoBehaviour {

    public static NPCEventManager instance = null;

    public Camera camera;
    public List<Event> eventList;
    private Event currentEvent;
    private NPCscript npc;

    //[System.Serializable]
    //public struct Event
    //{
    //    public string eventName;
    //    public bool isTriggered;
    //    public bool isFinished;
    //    public GameObject npcRef;
    //    public int diaLineNum;
        

    //    public Event(string name, GameObject obj, int num)
    //    {
    //        eventName = name;
    //        npcRef = obj;
    //        diaLineNum = num;
    //        isTriggered = false;
    //        isFinished = false;
    //    }
    //};

    [System.Serializable]
    public enum EventType
    {
        instant,
        delayed
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        eventList = new List<Event>();
    }

    // Use this for initialization
    void Start () {
        npc = GameObject.Find("NPC").GetComponent<NPCscript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TriggerEvent(string eventName, GameObject objRef, EventType type)
    {
        GreetPlayer.instance.Enter(npc);

        GreetPlayer.instance.Execute(npc);
    }

    public void Reset()
    {
        GreetPlayer.instance.Exit(npc);
    }

    //**************** NPC state ****************//
    public class GreetPlayer : Event<NPCscript>
    {
        public static GreetPlayer instance = new GreetPlayer();

        public override void Enter(NPCscript npc)
        {
            npc.ZoominCamera();
        }

        public override void Execute(NPCscript npc)
        {
            npc.Talk();
        }

        public override void Exit(NPCscript npc)
        {
            npc.ZoomoutCamera();
        }

        public override bool onMessage(NPCscript npc)
        {
            throw new NotImplementedException();
        }
    }
}
