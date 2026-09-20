using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using TMPro;

public class ExitOffice : MonoBehaviour
{
    public bool isExiting;
    public bool nearExit;
    public Canvas exitNotif;
    public PlayerInput playerInput;
    public Animator playerAnim;
    public Animator cameraAnim;
    public Marking marking;
    public Execution execution;

    public int currentDay = 1;
    public TMP_Text dayText;


    public PlayableDirector director;


    [System.Serializable]
    public struct Prisoner
    {
        public PrisonerSO prisonerName;
        public string prisonerNameString;
        public GameObject prisonerGameObject;
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
            if (isExiting)
            {
                exitNotif.enabled = false;
            }
            else
            {
                exitNotif.enabled = true;
            }
        }
        else
        {
            exitNotif.enabled = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (nearExit)
            {
                isExiting = true;

                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;

                playerInput.actions.FindAction("Movement").Disable();
                playerInput.actions.FindAction("Look").Disable();

                playerAnim.enabled = true;
                cameraAnim.enabled = true;

                PlayExitAnimation();
            }
        }
    }

    public void PlayExitAnimation()
    {
        currentDay++;
        if (currentDay == 2)
        {
            dayText.text = "Day " + currentDay.ToString();
            currentDay = 1;
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
                prisoner.prisonerGameObject.SetActive(false);
                marking.CheckDeadPrisoners(prisoner.prisonerNameString);
                execution.CheckDeadPrisoners(prisoner.prisonerNameString);
            }
        }
        
    }



    public void ControlReturn()
    {
        isExiting = false;

        playerAnim.enabled = false;
        cameraAnim.enabled = false;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();
    }
}

