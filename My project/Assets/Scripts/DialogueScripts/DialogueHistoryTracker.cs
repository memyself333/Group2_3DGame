using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHistoryTracker : MonoBehaviour
{
    public static DialogueHistoryTracker Instance;

    private readonly List<DialogueSO> spokenConvos = new List<DialogueSO>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
}
