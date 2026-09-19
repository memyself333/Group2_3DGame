using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class Marking : MonoBehaviour
{
    public bool isMarking = false;
    public bool nearPaper = false;
    public Canvas markCanva;
    public Canvas paperNotif;
    public TMP_Text prisonerName;
    public string currentPrisoner;
    public string currentPaper;
    public UnityEngine.UI.Toggle[] toggles;
    public int currentPrisonerPoints;

    public PlayerInput playerInput;

    public void Awake()
    {
        isMarking = false;
        markCanva.enabled = false;
    }

    public void Update()
    {
        if (nearPaper)
        {
            if(isMarking)
            {
                paperNotif.enabled = false;
            }
            else
            {
                paperNotif.enabled = true;
            }
            
        }
        else
        {
            paperNotif.enabled = false;
        }

    }
    //Disable playerInput, make cursor visible, and make marking canva visible
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (nearPaper)
            {
                foreach (var toggle in toggles)
                {
                    toggle.GetComponent<UnityEngine.UI.Toggle>().isOn = false;
                }
                currentPrisonerPoints = 0;
                isMarking = true;
                markCanva.enabled = true;
                prisonerName.text = currentPrisoner;

                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;

                playerInput.actions.FindAction("Movement").Disable();
                playerInput.actions.FindAction("Look").Disable();
            }
        }
    }

    public void ExitButtonPressed()
    {
        PrisonerSO[] allPrisonerSOs = Resources.LoadAll<PrisonerSO>("PrisonerSO");
        PrisonerSO matchedSO = allPrisonerSOs.FirstOrDefault(so => so.prisonerName == currentPrisoner);
        if (matchedSO != null)
        {
            matchedSO.prisonerPoints = currentPrisonerPoints;
        }
        else
        {
            Debug.Log("No matching ScriptableObject found.");
        }

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        isMarking = false;
        markCanva.enabled = false;
        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();
    }

    //Check toggle values and add them to the prisoner points in PrisonerSO
    public void GoodToggleValueChanged(UnityEngine.UI.Toggle toggle)
    {
        if (toggle.isOn)
            currentPrisonerPoints++;
        else currentPrisonerPoints--;
    }

    public void BadToggleValueChanged(UnityEngine.UI.Toggle toggle)
    {
        if (toggle.isOn)
            currentPrisonerPoints--;
        else currentPrisonerPoints++;
    }

}
