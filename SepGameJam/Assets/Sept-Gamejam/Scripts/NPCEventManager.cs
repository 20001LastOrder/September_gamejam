using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCEventManager : MonoBehaviour {

    public static NPCEventManager instance = null;

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

    }

    // Use this for initialization
    void Start () {
        npc = GameObject.Find("NPC").GetComponent<NPCscript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ObtainCubeTrigger()
    {
        MeetPlayerAtTheEnd.instance.SetLine(true);
    }

    public void FinishMeetUp()
    {
        GreetPlayer.instance.SetLine(true);
    }

    public void TriggerEvent(int stage)
    {
        Debug.Log(stage);
        switch (stage)
        {
            case 0:
                GreetPlayer.instance.Enter(npc);
                GreetPlayer.instance.Execute(npc);
                break;
            case 1:
                MeetPlayerAtTheEnd.instance.Enter(npc);
                MeetPlayerAtTheEnd.instance.Execute(npc);
                break;
        }
    }

    public void Reset()
    {
        GreetPlayer.instance.Exit(npc);
    }

    //**************** NPC state ****************//
    public class GreetPlayer : Event<NPCscript>
    {
        private static int firstMeetDiaLine = 4;
        private static int secondMeetDiaLine = 18;

        private int currentLine = firstMeetDiaLine;

        public static GreetPlayer instance = new GreetPlayer();

        public void SetLine(bool b)
        {
            if (b)
            {
                currentLine = secondMeetDiaLine;
            }
            else
            {
                currentLine = firstMeetDiaLine;
            }
        }

        public override void Enter(NPCscript npc)
        {
            npc.ZoominCamera();
        }

        public override void Execute(NPCscript npc)
        {
            npc.Talk(currentLine);
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

    // Final state: Meet player at the gate
    public class MeetPlayerAtTheEnd : Event<NPCscript>
    {
        private static int noCubeMeetDiaLine = 25;
        private static int hasCubeMeetDiaLine = 38;

        private int currentLine = noCubeMeetDiaLine;

        public static MeetPlayerAtTheEnd instance = new MeetPlayerAtTheEnd();

        public void SetLine(bool b)
        {
            if (b)
            {
                currentLine = hasCubeMeetDiaLine;
            }
            else
            {
                currentLine = noCubeMeetDiaLine;
            }
        }

        public override void Enter(NPCscript npc)
        {
            
            npc.ZoominCamera();
        }

        public override void Execute(NPCscript npc)
        {
            Debug.Log("end event trigger");
            npc.Talk(currentLine);
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
