using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public Canvas dialogueCanvas;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public Button[] choiceButtons;

    public bool isDialogueActive = false;

    public PlayerInput playerInput;

    public DialogueSO currentDialogue;
    private int dialogueIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        dialogueCanvas.enabled = false;

        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    //Start dialogue when talking to NPC
    public void StartDialogue(DialogueSO dialogueSO)
    {
        currentDialogue = dialogueSO;
        dialogueIndex = 0;
        isDialogueActive = true;
        ShowDialogue();

        if (dialogueIndex >= currentDialogue.lines.Length)
        {
            ShowChoices();
        }
    }

    //Determine if there is more conversation or if choices must be displayed
    public void AdvanceDialogue()
    {
        if (dialogueIndex < currentDialogue.lines.Length)
        {
            ShowDialogue();
        }
        else
        {
            ShowChoices();
        }
    }

    //Show the dialogue lines and disable other canavases and playerInput
    private void ShowDialogue()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DialogueLine line = currentDialogue.lines[dialogueIndex];

        DialogueHistoryTracker.Instance.RecordConvos(currentDialogue);

        actorName.text = line.speaker.actorName;

        dialogueText.text = line.text;

        dialogueCanvas.enabled = true;

        playerInput.actions.FindAction("Movement").Disable();
        playerInput.actions.FindAction("Look").Disable();
        dialogueIndex++;
    }


    //Show choices and check if out of options, so that they can display the "Farewell" option
    private void ShowChoices()
    {

        ClearChoices();

        if(currentDialogue.options.Length  > 0)
        {
            for (int i = 0; i < currentDialogue.options.Length; i++)
            {
                var option = currentDialogue.options[i];

                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = option.optionText;
                choiceButtons[i].gameObject.SetActive(true);

                choiceButtons[i].onClick.AddListener(() => ChooseOption(option.nextDialogue));
            }
            if (currentDialogue.options.Length < 4)
            {
                choiceButtons[3].GetComponentInChildren<TMP_Text>().text = "Farewell";
                choiceButtons[3].onClick.AddListener(EndDialogue);
                choiceButtons[3].gameObject.SetActive(true);
            }
        }
        else
        {
            choiceButtons[0].GetComponentInChildren<TMP_Text>().text = "Farewell";
            choiceButtons[0].onClick.AddListener(EndDialogue);
            choiceButtons[0].gameObject.SetActive(true);
        }
    }

    //After option is chosen
    private void ChooseOption(DialogueSO dialogueSO)
    {
        if(dialogueSO == null)
        {
            EndDialogue();
        }
        else
        {
            ClearChoices();
            StartDialogue(dialogueSO);
        }
    }

    //Turn off dialogue canvas and re-enable the other canavases and the playerInput
    public void EndDialogue()
    {
        dialogueIndex = 0;
        isDialogueActive = false;
        ClearChoices();

        dialogueCanvas.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();
    }

    //Makes sure previous choices don't linger
    private void ClearChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
    }

}
