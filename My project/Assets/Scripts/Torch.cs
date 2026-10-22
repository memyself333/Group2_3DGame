using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Torch : MonoBehaviour
{

    public GameObject intTorch;
    public GameObject holdTorch;
    public bool isLookingAtTorch;
    public bool nearTorch;
    public FPController fpController;
    public Image cursor;
    public Sprite cursorHand;
    public Sprite cursorReticle;
    public ExitOffice exitOffice;
    public ObjectInteract interact;
    public Execution execution;
    public PlayerInput playerInput;
    public GameObject notifTorch;
    public GameObject torchCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        intTorch.SetActive(false);
        holdTorch.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (nearTorch)
        {
            Debug.Log("Step1");
            Ray cursorRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit cursorHit;

            if (Physics.Raycast(cursorRay, out cursorHit))
            {
                Debug.Log("Step2");
                if (cursorHit.collider.CompareTag("Torch"))
                {
                    Debug.Log("Step3");
                    isLookingAtTorch = true;
                    cursor.sprite = cursorHand;
                }
                else
                {
                    isLookingAtTorch = false;
                    cursor.sprite = cursorReticle;
                }
            }
        }
        else if (exitOffice.nearExit || interact.CheckUserCloseToTable() || fpController.nearANPC || execution.nearExecuter)
        {

        }
        else
        {
            isLookingAtTorch = false;
            cursor.sprite = cursorReticle;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isLookingAtTorch)
            {
                cursor.sprite = cursorReticle;
                fpController.seenTorchTip = true;
                torchCollider.SetActive(false);
                notifTorch.SetActive(false);
                intTorch.SetActive(false);
                holdTorch.SetActive(true);
            }
        }
    }
}
