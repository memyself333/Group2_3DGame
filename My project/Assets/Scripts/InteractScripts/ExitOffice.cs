using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class ExitOffice : MonoBehaviour
{
    public bool isExiting;
    public bool nearExit;
    public Animator hintAnimator;
    public TMP_Text hintText;
    public PlayerInput playerInput;
    public Animator playerAnim;
    public Animator cameraAnim;
    public Marking marking;
    public Execution execution;
    public Torch torch;
    public Canvas hudCanvas;

    public Image cursor;
    public Sprite cursorReticle;
    public Sprite cursorHand;
    public bool isLookingAtExit = false;
    public ObjectInteract interact;
    public FPController fpController;

    public GameObject williamWall;
    public GameObject jumpscareBox;
    public GameObject[] triggerBoxes;
    public GameObject[] prisonCells;
    public Image williamCross;
    public Image williamBlood;
    public AudioSource musicPlayer;

    public int currentDay = 1;
    public TMP_Text dayText;

    public GameObject letterObject;
    public GameObject bookObject;
    public GameObject torchObject;
    public GameObject notifTorch;
    public GameObject torchCollider;

    public PlayableDirector director;


    [System.Serializable]
    public struct Prisoner
    {
        public PrisonerSO prisonerName;
        public string prisonerNameString;
        public GameObject prisonerGameObject;
        public List<DialogueSO> prisonerConvosDay1;
        public List<DialogueSO> prisonerConvosDay2;
        public List<DialogueSO> prisonerConvosDay3;
    }

    [SerializeField] public Prisoner[] prisoners;


    private void Awake()
    {
        isExiting = false;
        williamCross.enabled = false;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();

        playerAnim.enabled = false;
        cameraAnim.enabled = false;
        bookObject.SetActive(false);

        foreach (var bar in prisonCells)
        {
            bar.gameObject.SetActive(true);
        }
        williamWall.SetActive(true);
        foreach (var boxes in triggerBoxes)
        {
            boxes.gameObject.SetActive(false);
        }
        jumpscareBox.SetActive(false);

    }

    void Update()
    {
        if (nearExit)
        {
            Ray cursorRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit cursorHit;
            if (Physics.Raycast(cursorRay, out cursorHit))
            {
                if (cursorHit.collider.CompareTag("Exit"))
                {
                    isLookingAtExit = true;
                    cursor.sprite = cursorHand;
                }
                else
                {
                    isLookingAtExit = false;
                    cursor.sprite = cursorReticle;
                }
            }
        }
        else if (execution.nearExecuter || interact.CheckUserCloseToTable() || fpController.nearANPC || torch.nearTorch)
        {

        }
        else
        {
            isLookingAtExit = false;
            cursor.sprite = cursorReticle;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isLookingAtExit)
            {
                if (execution.executionDone)
                {
                    fpController.seenExitTip = true;
                    isExiting = true;

                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                    UnityEngine.Cursor.visible = true;

                    playerInput.actions.FindAction("Movement").Disable();
                    playerInput.actions.FindAction("Look").Disable();

                    playerAnim.enabled = true;
                    cameraAnim.enabled = true;

                    PlayExitAnimation();
                }
                else
                {
                    StartCoroutine(DisplayTip());
                }
            }
        }
    }

    public void PlayExitAnimation()
    {
        hudCanvas.enabled = false;
        marking.ResetMarking();
        execution.ResetGeneral();
        execution.hasTalked = false;
        currentDay++;
        dayText.text = "Day " + currentDay.ToString();

        director.Play();
        foreach (var prisoner in prisoners)
        {
            if (prisoner.prisonerName.isAlive)
            {
                prisoner.prisonerGameObject.SetActive(true);
            }
            else
            {
                if (prisoner.prisonerNameString == "William Atkinson")
                {
                    prisoner.prisonerName.isAlive = true;
                    prisoner.prisonerGameObject.SetActive(true);
                    williamCross.enabled = true;
                    williamBlood.enabled = false;
                }
                else
                {
                    prisoner.prisonerGameObject.SetActive(false);
                    marking.CheckDeadPrisoners(prisoner.prisonerNameString);
                    execution.ResetGeneral();
                    execution.CheckDeadPrisoners(prisoner.prisonerNameString);
                    
                }
                
            }
        }
        
        if (currentDay == 1)
        {
            letterObject.SetActive(true);
            bookObject.SetActive(false);
        }
        else if (currentDay == 2)
        {
            letterObject.SetActive(true);
            bookObject.SetActive(false);  
        }
        else if (currentDay == 3)
        {
            letterObject.SetActive(false);
            bookObject.SetActive(true);
        }
        else if (currentDay == 4)
        {
            letterObject.SetActive(true);
            bookObject.SetActive(false);
            torchObject.SetActive(true);
            notifTorch.SetActive(true);
            torchCollider.SetActive(true);

            foreach (var prisoner in prisoners)
            {
                prisoner.prisonerGameObject.SetActive(false);
            }
            foreach (var bar in prisonCells)
            {
                bar.gameObject.SetActive(false);
            }
            williamWall.SetActive(false);
            foreach (var boxes in triggerBoxes)
            {
                boxes.gameObject.SetActive(true);
            }
            jumpscareBox.SetActive(true);
            
        }

        musicPlayer.Stop();
    }



    public void ControlReturn()
    {

        execution.executionDone = false;
        hudCanvas.enabled = true;
        isExiting = false;

        playerAnim.enabled = false;
        cameraAnim.enabled = false;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();

        for (int i = 0; i < prisoners.Length; i++)
        {
            if (currentDay == 1)
            {
                prisoners[i].prisonerGameObject.GetComponent<NPC_Talk>().conversations = prisoners[i].prisonerConvosDay1;
            }
            else if (currentDay == 2)
            {
                prisoners[i].prisonerGameObject.GetComponent<NPC_Talk>().conversations = prisoners[i].prisonerConvosDay2;
            }
            else if (currentDay == 3)
            {
                prisoners[i].prisonerGameObject.GetComponent<NPC_Talk>().conversations = prisoners[i].prisonerConvosDay3;
            }
        }

    }
    public IEnumerator DisplayTip()
    {
        hintAnimator.SetBool("ShowHint", true);
        hintAnimator.SetBool("HideHint", false);
        hintAnimator.SetBool("Off", false);
        if (currentDay == 4)
        {
            hintText.text = "You cannot leave.";
        }
        else
        {
            hintText.text = "You must execute a prisoner before you can exit the prison.";
        }
        yield return new WaitForSeconds(3f);
        hintAnimator.SetBool("ShowHint", false);
        hintAnimator.SetBool("HideHint", true);
        hintAnimator.SetBool("Off", false);
    }

}

