using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using System.Collections.Generic;

public class NPC_Talk : MonoBehaviour
{
    //public Animator animator;
    public Controls controls;
    public List<DialogueSO> conversations;
    public DialogueSO currentConversation;
    public bool nearNPC = false;
    public GameObject thisNPC;

    private void Awake()
    {
        //animator = GetComponent<Animator>(); These will be added when models are added
    }
    public void OnEnable()
    {
        //animator.Play("Stand"); These will be added when models are added
    }

    public void OnDisable()
    {
        //animator.Play("Sit"); These will be added when models are added
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (nearNPC)
            {
                if (DialogueManager.Instance.isDialogueActive)
                {
                    DialogueManager.Instance.AdvanceDialogue();
                }
                else
                {
                    CheckForNewConversation();
                    DialogueManager.Instance.StartDialogue(currentConversation);
                }
            }
        }
    }

    private void CheckForNewConversation()
    {
        for (int i= 0; i < conversations.Count; i++)
        {
            var convo = conversations[i];
            if (convo != null && convo.IsConditionMet())
            {
                conversations.RemoveAt(i);
                currentConversation = convo;
            }
        }
    }

}
