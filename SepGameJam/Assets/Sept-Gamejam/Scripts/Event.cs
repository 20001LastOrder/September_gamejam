using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Event <NpcType> {

    abstract public void Enter(NpcType npc);

    abstract public void Execute(NpcType npc);

    abstract public void Exit(NpcType npc);

    // potentially one more signature should be the message type
    abstract public bool onMessage(NpcType npc);
	
}
