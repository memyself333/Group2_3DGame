using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHistoryTracker : MonoBehaviour
{
    public static DialogueHistoryTracker Instance;
    public DialogueSO[] day1Convos;
    public DialogueSO[] day2Convos;
    public DialogueSO[] day3Convos;
    public PrisonerSO[] prisoners;
    public ExitOffice exitOffice;

    public readonly List<DialogueSO> spokenConvos = new List<DialogueSO>();
    public Execution execution;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Update()
    {
        if (exitOffice.currentDay == 1)
        {
            if (spokenConvos.Contains(day1Convos[0]) && spokenConvos.Contains(day1Convos[1]) && spokenConvos.Contains(day1Convos[2]) && spokenConvos.Contains(day1Convos[3]))
            {
                execution.hasTalked = true;
            }
        }
        else if (exitOffice.currentDay == 2)
        {
            if (spokenConvos.Contains(day2Convos[3]))
            {
                if (exitOffice.currentDay == 2)
                {
                    if (!prisoners[0].isAlive)
                    {
                        if (spokenConvos.Contains(day2Convos[1]) && spokenConvos.Contains(day2Convos[2]))
                        {
                            execution.hasTalked = true;
                        }
                    }
                    else if (!prisoners[1].isAlive)
                    {
                        if (spokenConvos.Contains(day2Convos[0]) && spokenConvos.Contains(day2Convos[2]))
                        {
                            execution.hasTalked = true;
                        }
                    }
                    else if (!prisoners[2].isAlive)
                    {
                        if (spokenConvos.Contains(day2Convos[0]) && spokenConvos.Contains(day2Convos[1]))
                        {
                            execution.hasTalked = true;
                        }
                    }
                }
            }
        }
        else if (exitOffice.currentDay == 3)
        {
            if (spokenConvos.Contains(day3Convos[3]))
            {
                if (!prisoners[0].isAlive && !prisoners[1].isAlive)
                {
                    if (spokenConvos.Contains(day3Convos[2]))
                    {
                        execution.hasTalked = true;
                    }
                }
                else if (!prisoners[0].isAlive && !prisoners[2].isAlive)
                {
                    if (spokenConvos.Contains(day3Convos[1]))
                    {
                        execution.hasTalked = true;
                    }
                }
                else if (!prisoners[1].isAlive && !prisoners[2].isAlive)
                {
                    if (spokenConvos.Contains(day3Convos[0]))
                    {
                        execution.hasTalked = true;
                    }
                }
            }
        }
    }


    public void RecordConvos (DialogueSO dialogueSO)
    {
        spokenConvos.Add(dialogueSO);

        Debug.Log("Just had convo " + dialogueSO);
    }

    public bool HasExperienced (DialogueSO dialogueSO)
    {
        return spokenConvos.Contains(dialogueSO);
    }



    //Used to check if the player has already talked to a prisoner.
}
