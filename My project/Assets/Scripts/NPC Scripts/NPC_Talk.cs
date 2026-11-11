using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
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
    public Execution execution;
    public FPController playerController;
    public Animator npcAnim;

    private void Awake()
    {
        npcAnim = this.gameObject.GetComponent<Animator>();
        npcAnim.SetBool("Talk", false);
        npcAnim.SetBool("Idle", true);
    }
    public void OnEnable()
    {
        //animator.Play("Stand"); These will be added when models are added
    }

    public void OnDisable()
    {
        //animator.Play("Sit"); These will be added when models are added
    }

    //Checks when NPC is interacted with to start dialogue
    public void OnTalk(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Step1");
            if (playerController.npcTalk.nearNPC)
            {
                Debug.Log("Step2");
                if (playerController.npcTalk == this.gameObject.GetComponent<NPC_Talk>())
                {
                    if (playerController.isLookingAtNPC)
                    {
                        Debug.Log("Step3");
                        playerController.seenTalkTip = true;
                        execution.hasTalked = true;
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
                else
                {
                    Debug.Log("fail to find player npc talk");
                }
            }
            else
            {
                Debug.Log("Step4");
            }
        }
    }



    //Checks if any conditions need to be met for any conversations
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
