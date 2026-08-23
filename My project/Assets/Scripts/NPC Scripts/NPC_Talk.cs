using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class NPC_Talk : MonoBehaviour
{
    //public Animator animator;
    public Controls controls;
    public DialogueSO dialogueSO;
    public bool nearNPC = false;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
    }
    public void OnEnable()
    {
        //animator.Play("Stand");
    }

    public void OnDisable()
    {
        //animator.Play("Sit");
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
                    DialogueManager.Instance.StartDialogue(dialogueSO);
                }
            }
        }
    }

}
