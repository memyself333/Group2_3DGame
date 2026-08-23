using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public enum NPCState { Default, Idle, Talk }
    public NPCState currentState = NPCState.Idle;
    private NPCState defaultState;

    public NPC_Talk talk;

    void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
    }

    public void SwitchState(NPCState newState)
    {         
        currentState = newState;
        
        talk.enabled = newState == NPCState.Talk;
    }

    public void NPCTalk()
    {
        if (currentState == NPCState.Idle)
        {
            SwitchState(NPCState.Talk);
        }
        else if (currentState == NPCState.Talk)
        {
            SwitchState(NPCState.Idle);
        }
    }

}
