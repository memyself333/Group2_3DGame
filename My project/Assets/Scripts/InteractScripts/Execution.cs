using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Execution : MonoBehaviour
{
    public bool isExecuting;
    public bool nearExecuter;
    public Canvas executeCanva;
    public Canvas executeNotif;
    public TMP_Text[] prisonerNames;
    public TMP_Text[] prisonerScores;
    public PrisonerSO[] prisoners;

    public PlayerInput playerInput;

    private void Awake()
    {
        isExecuting = false;
        executeCanva.enabled = false;

    }

    public void Start()
    {
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
        for (int i = 0; i < prisonerScores.Length; i++)
        {
            prisonerScores[i].text = prisoners[i].prisonerPoints.ToString();
        }
        if (nearExecuter)
        {
            if (isExecuting)
            {
                executeNotif.enabled = false;
            }
            else
            {
                executeNotif.enabled = true;
            }
        }
        else
        {
            executeNotif.enabled = false;
        }
    }

    //Disable playerInput, make cursor visible, and make execute canva visible
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (nearExecuter)
            {
                isExecuting = true;
                executeCanva.enabled = true;

                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;

                playerInput.actions.FindAction("Movement").Disable();
                playerInput.actions.FindAction("Look").Disable();

            }
        }
    }

    public void ExitButtonPressed()
    {
        isExecuting = false;
        executeCanva.enabled = false;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();

    }
    //Checks when toggles are changed, and applying the changed value to the PrisonerSO
    public void ExecutionToggleChanged(UnityEngine.UI.Toggle toggle)
    {
        if (toggle.isOn)
        {
            PrisonerSO[] allPrisonerSOs = Resources.LoadAll<PrisonerSO>("PrisonerSO");
            PrisonerSO matchedSO = allPrisonerSOs.FirstOrDefault(so => so.prisonerName == toggle.gameObject.name);
            if (matchedSO != null)
            {
                matchedSO.isAlive = false;
            }
            else
            {
                Debug.Log("No matching ScriptableObject found.");
            }
        }
        else
        {
            PrisonerSO[] allPrisonerSOs = Resources.LoadAll<PrisonerSO>("PrisonerSO");
            PrisonerSO matchedSO = allPrisonerSOs.FirstOrDefault(so => so.prisonerName == toggle.gameObject.name);
            if (matchedSO != null)
            {
                matchedSO.isAlive = true;
            }
            else
            {
                Debug.Log("No matching ScriptableObject found.");
            }
        }
    }
}
