using JetBrains.Annotations;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Execution : MonoBehaviour
{
    public GameObject executionBox;
    public GameObject exitBox;
    public bool isExecuting;
    public bool nearExecuter;
    public Canvas executeCanva;
    public Canvas hudCanvas;
    public TMP_Text[] prisonerNames;
    public TMP_Text[] prisonerScoresTexts;
    public int[] prisonerScores;
    public Image[] prisonerBlood;
    public PrisonerSO[] prisoners;
    public bool executionDone = false;
    public ExitOffice exitOffice;
    public ObjectInteract interact;
    public FPController fpController;
    public GameObject exitButton;
    public bool hasTalked = false;
    public bool isLookingAtPaper = false;
    public Image cursor;
    public Sprite cursorReticle;
    public Sprite cursorQuill;
    public Torch torch;


    public Animator hintAnimator;
    public TMP_Text hintText;
    public bool williamAttemptedExecution;
    public TMP_Text[] behaviours;

    public int lastClipIndex1 = -1;

    public PlayerInput playerInput;

    public Animator[] bloodAnimations;

    [System.Serializable]
    public struct PrisonerAlive
    {
        public string prisonerName;
        public PrisonerSO prisonerSO;
        public Button button;
    }

    [SerializeField] public PrisonerAlive[] prisonerAlives;

    private void Awake()
    {
        isExecuting = false;
        executeCanva.enabled = false;
    }

    public void Start()
    {
        executionDone = false;
        foreach (var prisoner in prisoners)
        {
            prisoner.prisonerPoints = 0;
            prisoner.isAlive = true;
        }
    }


    //assigning prisoner names and scores to the textboxes in the canva
    public void Update()
    {
        for (int i = 0; i < prisonerNames.Length; i++)
        {
            prisonerNames[i].text = prisoners[i].prisonerName;
        }

        if (nearExecuter)
        {
            Ray cursorRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit cursorHit;

            if (Physics.Raycast(cursorRay, out cursorHit))
            {
                if (cursorHit.collider.CompareTag("ExecutionPaper"))
                {
                    isLookingAtPaper = true;
                    cursor.sprite = cursorQuill;
                }
                else
                {
                    isLookingAtPaper = false;
                    cursor.sprite = cursorReticle;
                }
            }
        }
        else if (exitOffice.nearExit || interact.CheckUserCloseToTable() || fpController.nearANPC || torch.nearTorch)
        {

        }
        else
        {
            isLookingAtPaper = false;
            cursor.sprite = cursorReticle;
        }
    }

    //Disable playerInput, make cursor visible, and make execute canva visible
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isLookingAtPaper)
            {
                if (hasTalked)
                {
                    for (int i = 0; i < prisonerScoresTexts.Length; i++)
                    {
                        prisonerScores[i] = prisoners[i].prisonerPoints;
                        if (prisonerScores[i] < 2)
                        {
                            prisonerScoresTexts[i].text = "Misbehaved";
                        }
                        else if (prisonerScores[i] == 2)
                        {
                            prisonerScoresTexts[i].text = "Neutral";
                        }
                        else if (prisonerScores[i] > 2)
                        {
                            prisonerScoresTexts[i].text = "Well Behaved";
                        }
                        
                        if (!prisonerAlives[i].prisonerSO.isAlive)
                        {
                            prisonerScoresTexts[i].text = "Deceased";
                        }
                    }

                    if (williamAttemptedExecution)
                    {
                        prisonerScoresTexts[3].text = "Nihil";
                    }

                    fpController.seenExecuteTip = true;
                    isExecuting = true;
                    executeCanva.enabled = true;
                    SoundManager.instance.PlayRandomSound(SoundManager.instance.objtIntClips, -1, 3, 1, SoundManager.instance.audioSources[2]);

                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                    UnityEngine.Cursor.visible = true;

                    hudCanvas.enabled = false;

                    playerInput.actions.FindAction("Movement").Disable();
                    playerInput.actions.FindAction("Look").Disable();

                    exitButton.SetActive(false);
                }
                else
                {
                    StartCoroutine(DisplayTip());
                }

            }
        }
    }

    public void ExitButtonPressed()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.menuClips[3], 1, SoundManager.instance.audioSources[1], false, false);
        isExecuting = false;
        executeCanva.enabled = false;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        hudCanvas.enabled = true; 

        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();

        executionDone = true;
        nearExecuter = false;
        executionBox.SetActive(false);
    }

    public void CheckDeadPrisoners(string pName)
    {
        for (int i = 0; i < prisoners.Length; i++)
        {
            if (prisoners[i].prisonerName == pName)
            {
                prisonerAlives[i].button.interactable = false;
                prisonerBlood[i].enabled = true;
                behaviours[i].text = "Deceased";
            }
        }


    }

    public void ResetGeneral()
    {
        for (int i = 0; i < prisoners.Length; i++)
        {
            prisonerAlives[i].button.interactable = true;
        }

        if (williamAttemptedExecution)
        {
            prisonerAlives[3].button.interactable = false;
            prisonerAlives[1].button.interactable = false;
            prisonerBlood[1].enabled = true;
            exitOffice.prisoners[1].prisonerName.isAlive = false;
        }
    }

    public void ExecutePrisoner(UnityEngine.UI.Button button)
    {
        for (int i = 0; i < prisonerAlives.Length; i++)
        {
            if (prisonerAlives[i].button == button)
            {
                SoundManager.instance.PlaySound(SoundManager.instance.menuClips[4], 1, SoundManager.instance.audioSources[1], false, false);
                SoundManager.instance.StopSound(SoundManager.instance.audioSources[0], false, 2f);
                prisonerAlives[i].prisonerSO.isAlive = false;
                prisonerBlood[i].enabled = true;
                bloodAnimations[i].Play("P" + (i + 1) + "Blood");
                prisonerAlives[i].button.animator.SetTrigger("Selected");
                prisonerAlives[i].button.interactable = false;
                if (prisonerAlives[i].prisonerName == "William Atkinson")
                {
                    williamAttemptedExecution = true;
                }
            }
            else
            {
                prisonerAlives[i].button.interactable = false;
            }
        }
        exitButton.SetActive(true);
    }

    public IEnumerator DisplayTip()
    {
        hintAnimator.SetBool("ShowHint", true);
        hintAnimator.SetBool("HideHint", false);
        hintAnimator.SetBool("Off", false);
        if (exitOffice.currentDay == 4)
        {
            hintText.text = "You can't kill him.";
        }
        else
        {
            hintText.text = "You must talk to the prisoners first, before you can execute them.";
        }
        yield return new WaitForSeconds(3f);
        hintAnimator.SetBool("ShowHint", false);
        hintAnimator.SetBool("HideHint", true);
        hintAnimator.SetBool("Off", false);
    }
}
