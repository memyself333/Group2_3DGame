using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    public GameObject notifPrisoner1;
    public GameObject notifP1Paper;
    public GameObject notifPrisoner2;
    public GameObject notifExecute;
    public NPC_Talk npcTalk;
    public Marking marking;
    public Execution execution;
    public Canvas npcCanvas;
    public Canvas hudCanvas;
    public bool isTalking;
    public DialogueManager dm;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        isTalking = dm.isDialogueActive;

        HandleMovement();
        HandleLook();

        if (npcTalk.nearNPC)
        {
            if (isTalking)
            {
                hudCanvas.enabled = false;
                npcCanvas.enabled = false;
            }
            else
            {
                hudCanvas.enabled = true;
                npcCanvas.enabled = true;
            }
        }
        else
        {
            hudCanvas.enabled = true;
            npcCanvas.enabled = false;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, - verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnTriggerEnter(Collider other)
    {
        var foundNpcTalk = other.GetComponentInParent<NPC_Talk>();
        if (foundNpcTalk != null)
        {
            Debug.Log("Found npc_talk");
            npcTalk = foundNpcTalk;
            npcTalk.nearNPC = true;
            return;
        }
        else
        {
            Debug.Log("Not found npc_talk");
        }

        if (other.gameObject == notifP1Paper)
        {
            marking.nearPaper = true;
            marking.currentPrisoner = other.gameObject.name;
        }

        if (other.gameObject == notifExecute)
        {
            execution.nearExecuter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == notifPrisoner1 || other.gameObject == notifPrisoner2)
        {
            npcTalk.nearNPC = false;
        }

        if (other.gameObject == notifP1Paper)
        {
            marking.nearPaper = false;
        }

        if (other.gameObject == notifExecute)
        {
            execution.nearExecuter = false;
        }
    }

}
