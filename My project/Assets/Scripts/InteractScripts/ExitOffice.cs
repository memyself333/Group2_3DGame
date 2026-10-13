using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using TMPro;
using System.Collections;

public class ExitOffice : MonoBehaviour
{
    public bool isExiting;
    public bool nearExit;
    public Canvas notifCanvas;
    public TMP_Text notifText;
    public PlayerInput playerInput;
    public Animator playerAnim;
    public Animator cameraAnim;
    public Marking marking;
    public Execution execution;
    public Canvas hudCanvas;
    public TMP_Text readText;
    [TextArea(3, 5)] public string textDay1;
    [TextArea(3, 5)] public string textDay2;

    public Image cursor;
    public Sprite cursorReticle;
    public Sprite cursorHand;
    public bool isLookingAtExit = false;
    public ObjectInteract interact;
    public FPController fpController;


    public int currentDay = 1;
    public TMP_Text dayText;

    public GameObject letterObject;
    public GameObject bookObject;

    public PlayableDirector director;


    [System.Serializable]
    public struct Prisoner
    {
        public PrisonerSO prisonerName;
        public string prisonerNameString;
        public GameObject prisonerGameObject;
        public List<DialogueSO> prisonerConvosDay1;
        public List<DialogueSO> prisonerConvosDay2;
    }

    [SerializeField] private Prisoner[] prisoners;


    private void Awake()
    {
        isExiting = false;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();

        playerAnim.enabled = false;
        cameraAnim.enabled = false;
        bookObject.SetActive(false);
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
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
        else if (execution.nearExecuter || interact.CheckUserCloseToTable() || fpController.nearANPC)
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
        if (currentDay == 3)
        {
            dayText.text = "The End";
            dayText.fontSize = 600;
        }
        else
        {
            dayText.text = "Day " + currentDay.ToString();
        }

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
            readText.text = textDay1;
        }
        else if (currentDay == 2)
        {
            letterObject.SetActive(false);
            bookObject.SetActive(true);
            readText.text = textDay2;   
        }
    }



    public void ControlReturn()
    {

        if (currentDay == 3)
        {
            Application.Quit();
        }
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
        }

    }
    public IEnumerator DisplayTip()
    {
        notifCanvas.enabled = true;
        notifText.text = "I must execute a prisoner first";
        yield return new WaitForSeconds(3f);
        notifCanvas.enabled = false;
    }
}

