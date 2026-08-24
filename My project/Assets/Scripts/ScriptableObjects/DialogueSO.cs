using UnityEngine;
[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueOption[] options;

    [Header("Conditional Requirements (Optional)")]
    public DialogueSO[] requiredConvos;

    public bool IsConditionMet()
    {
        if(requiredConvos.Length > 0)
        {
            foreach (var convo in requiredConvos)
            {
                if(!DialogueHistoryTracker.Instance.HasExperienced(convo))
                {
                    return false;
                }
            }    
        }
        return true;
    }
}

[System.Serializable]
public class DialogueLine
{
    public ActorSO speaker;
    [TextArea(3, 5)] public string text;
}

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public DialogueSO nextDialogue;
}
