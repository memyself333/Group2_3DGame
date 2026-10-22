using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using ChristinaCreatesGames.Typography.Typewriter;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public Canvas dialogueCanvas;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public Button[] choiceButtons;
    public Canvas hudCanvas;
    public Animator dialogueCanvasAnim;
    [SerializeField] private TypewriterEffect dialogueTypewriter;

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

    public void Update()
    {

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




    //Opened from DialogueBox_Open Anim
    private void ShowDialogue()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        hudCanvas.enabled = false;

        DialogueLine line = currentDialogue.lines[dialogueIndex];

        DialogueHistoryTracker.Instance.RecordConvos(currentDialogue);

        actorName.text = line.speaker.actorName;

        dialogueTypewriter.SetText(line.text);

        dialogueCanvas.enabled = true;

        //DialogueBox_Open Anim

        playerInput.actions.FindAction("Movement").Disable();
        playerInput.actions.FindAction("Look").Disable();


        dialogueIndex++;
    }


    //Show choices and check if out of options, so that they can display the "Farewell" option
    private void ShowChoices()
    {

        ClearChoices();


        if (currentDialogue.options.Length  > 0)
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

    public void EndDialogue()
    {

        dialogueIndex = 0;
        isDialogueActive = false;
        ClearChoices();

        //DialogueBox_Close Anim (should close before canvas is disabled)

        dialogueCanvas.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        hudCanvas.enabled = true;

        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();

        
    }

    private void ClearChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
    }

}
