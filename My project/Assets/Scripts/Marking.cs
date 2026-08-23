using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class Marking : MonoBehaviour
{
    public bool isMarking = false;
    public Canvas markCanva;
    public Canvas talkCanva;
    public string currentSpeaker;
    public int currentPrisonerPoints;
    public NPC_Talk npcTalk;

    public void Awake()
    {
        isMarking = false;
        markCanva.enabled = false;
    }
    public void MarkButtonPressed()
    {
        //camera move down
        //ui anim to appear
        isMarking = true;
        markCanva.enabled = true;
        talkCanva.enabled = false;
        currentPrisonerPoints = 0;
    }

    public void ExitButtonPressed()
    {
        PrisonerSO[] allPrisonerSOs = Resources.LoadAll<PrisonerSO>("Group2_3DGame\\My project\\Assets\\Scripts\\NPC Scripts\\PrisonerSO");
        PrisonerSO matchedSO = allPrisonerSOs.FirstOrDefault(so => so.prisonerName == currentSpeaker);
        if (matchedSO != null)
        {
            matchedSO.prisonerPoints = currentPrisonerPoints;
        }
        else
        {
            Debug.Log("No matching ScriptableObject found.");
        }

        isMarking = false;
        markCanva.enabled = false;
        talkCanva.enabled = false;



    }
    public void GoodToggleValueChanged(UnityEngine.UI.Toggle toggle)
    {
        if (toggle.isOn)
            currentPrisonerPoints++;
        else currentPrisonerPoints--;
    }

    public void BadToggleValueChanged(UnityEngine.UI.Toggle toggle)
    {
        if (toggle.isOn)
            currentPrisonerPoints--;
        else currentPrisonerPoints++;
    }

    public void Update()
    {
        if (isMarking)
        currentSpeaker = npcTalk.npcName;
    }
}
