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
    public TMP_Text[] prisonerScoresTexts;
    public int[] prisonerScores;
    public PrisonerSO[] prisoners;
    

    public PlayerInput playerInput;

    [System.Serializable]
    public struct PrisonerAlive
    {
        public string prisonerName;
        public PrisonerSO prisonerSO;
        public UnityEngine.UI.Toggle toggle;
        public GameObject breakLine;
    }

    [SerializeField] private PrisonerAlive[] prisonerAlives;

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
        }
        for (int i = 0; i < prisonerAlives.Length; i++)
        {
            prisonerAlives[i].prisonerSO.isAlive = !prisonerAlives[i].toggle.isOn;
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

    public void CheckDeadPrisoners(string pName)
    {
        for (int i = 0; i < prisoners.Length; i++)
        {
            if (prisoners[i].prisonerName == pName)
            {
                prisonerAlives[i].toggle.isOn = false;
                prisonerAlives[i].toggle.interactable = false;
                prisonerAlives[i].breakLine.SetActive(true);

            }
        }
    }
}
