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
    public TMP_Text prisonerName;
    public string currentPrisoner;
    public string currentPaper;
    public UnityEngine.UI.Toggle[] toggles;
    public int currentPrisonerPoints;
    private int currentPrisonerIndex = 0;

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

            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

            playerInput.actions.FindAction("Movement").Disable();
            playerInput.actions.FindAction("Look").Disable();
        }
    }

    public void ExitButtonPressed()
    {
        for (int i = 0; i < prisoners.Length; i++)
        {
            int goodPoints = prisoners[i].isToggleOn.Count(b => b);
            prisoners[i].prisonerSO.prisonerPoints = goodPoints;
        }
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        isMarking = false;
        markCanva.enabled = false;
        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();
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

}
