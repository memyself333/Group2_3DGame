using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static ExitOffice;
public class Marking : MonoBehaviour
{
    public bool isMarking = false;
    public bool nearPaper = false;
    public Canvas markCanva;
    public Canvas hudCanvas;
    public TMP_Text prisonerName;
    public string currentPrisoner;
    public string currentPaper;
    public UnityEngine.UI.Toggle[] toggles;
    public int currentPrisonerPoints;
    private int currentPrisonerIndex = 0;
    public GameObject[] menus;
    public TMP_Text[] questions;
    public CanvasGroup currentMenu;
    public ExitOffice exitOffice;

    public string[] day1Questions;
    public string[] day2Questions;
    public string[] day3Questions;
    public string[] day4Questions;


    public PlayerInput playerInput;


    [System.Serializable]
    public struct Prisoner
    {         
        public string prisonerName;
        public PrisonerSO prisonerSO;
        public bool[] isToggleOn;
    }

    [SerializeField] private Prisoner[] prisoners;

    public void Awake()
    {
        isMarking = false;
        markCanva.enabled = false;
        currentPrisoner = prisoners[0].prisonerName;
        foreach (var toggle in toggles)
        {
            toggle.isOn = false;
        }
    }

    public void Update()
    {
        foreach (GameObject canva in menus)
        {
            if (!isMarking)
            {
                if (canva.GetComponent<Canvas>().enabled == true)
                {
                    currentMenu = canva.GetComponent<CanvasGroup>();
                    break;
                }
                else
                {
                    currentMenu = null;
                }
            }
        }
        for (int i = 0; i < prisoners.Length; i++)
        {
            if (prisoners[i].prisonerName == currentPrisoner)
            {
                for (int j = 0; j < prisoners[i].isToggleOn.Length; j++)
                {
                    prisoners[i].isToggleOn[j] = toggles[j].isOn;
                }
            }
        }
    }
    //Disable playerInput, make cursor visible, and make marking canva visible
    public void OnMark(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentMenu != null)
            {
                currentMenu.alpha = 0;
                currentMenu.blocksRaycasts = false;
                currentMenu.interactable = false;
            }
            isMarking = true;
            markCanva.enabled = true;
            prisonerName.text = currentPrisoner;

            for (int i = 0; i < prisoners.Length; i++)
            {
                if (prisoners[i].prisonerName == currentPrisoner)
                {
                    for (int j = 0; j < prisoners[i].isToggleOn.Length; j++)
                    {
                        toggles[j].isOn = prisoners[i].isToggleOn[j];
                    }
                }
            }

            for (int i = 0; i < questions.Length; i++)
            {
                if (exitOffice.currentDay == 1)
                {
                    questions[i].text = day1Questions[i];
                }
                else if (exitOffice.currentDay == 2)
                {
                    questions[i].text = day2Questions[i];
                }
                else if (exitOffice.currentDay == 3)
                {
                    questions[i].text = day3Questions[i];
                }
                else if (exitOffice.currentDay == 4)
                {
                    questions[i].text = day4Questions[i];
                }
            }

            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

            hudCanvas.enabled = false;

            playerInput.actions.FindAction("Movement").Disable();
            playerInput.actions.FindAction("Look").Disable();
        }
    }

    public void ExitButtonPressed()
    {
        if (currentMenu != null)
        {
            currentMenu.alpha = 1;
            currentMenu.blocksRaycasts = true;
            currentMenu.interactable = true;
        }
        for (int i = 0; i < prisoners.Length; i++)
        {
            int goodPoints = prisoners[i].isToggleOn.Count(b => b);
            prisoners[i].prisonerSO.prisonerPoints = goodPoints;
        }
        isMarking = false;
        markCanva.enabled = false;

        if (currentMenu == null)
        { 
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;

            hudCanvas.enabled = true;

            playerInput.actions.FindAction("Movement").Enable();
            playerInput.actions.FindAction("Look").Enable();
        }
    }

    public void RightArrowPressed()
    {
        currentPrisonerIndex = (currentPrisonerIndex + 1) % prisoners.Length;
        currentPrisoner = prisoners[currentPrisonerIndex].prisonerName;
        prisonerName.text = currentPrisoner;

        for (int i = 0; i < prisoners.Length; i++)
        {
            if (prisoners[i].prisonerName == currentPrisoner)
            {
                for (int j = 0; j < prisoners[i].isToggleOn.Length; j++)
                {
                    toggles[j].isOn = prisoners[i].isToggleOn[j];
                }
            }
        }
    }

    public void LeftArrowPressed()
    {
        currentPrisonerIndex = (currentPrisonerIndex <= 0) ? prisoners.Length - 1 : currentPrisonerIndex - 1;
        currentPrisoner = prisoners[currentPrisonerIndex].prisonerName;
        prisonerName.text = currentPrisoner;

        for (int i = 0; i < prisoners.Length; i++)
        {
            if (prisoners[i].prisonerName == currentPrisoner)
            {
                for (int j = 0; j < prisoners[i].isToggleOn.Length; j++)
                {
                    toggles[j].isOn = prisoners[i].isToggleOn[j];
                }
            }
        }
    }

    public void CheckDeadPrisoners(string pName)
    {
        prisoners = prisoners.Where(prisoner => prisoner.prisonerName != pName).ToArray();
        currentPrisonerIndex = 0;
        currentPrisoner = prisoners[currentPrisonerIndex].prisonerName;
        prisonerName.text = currentPrisoner;

        for (int i = 0; i < prisoners.Length; i++)
        {
            if (prisoners[i].prisonerName == currentPrisoner)
            {
                for (int j = 0; j < prisoners[i].isToggleOn.Length; j++)
                {
                    toggles[j].isOn = prisoners[i].isToggleOn[j];
                }
            }
        }
    }

    public void ResetMarking()
    {
        currentPrisoner = prisoners[0].prisonerName;
        foreach (var prisoner in prisoners)
        {
            for (int j = 0; j < prisoner.isToggleOn.Length; j++)
            {
                prisoner.isToggleOn[j] = false;
            }
        }
    }
}
