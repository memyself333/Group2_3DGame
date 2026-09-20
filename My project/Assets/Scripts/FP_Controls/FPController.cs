using System;
using UnityEngine;
using UnityEngine.Audio;
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

    public AudioClip[] footstepStoneClips;
    public AudioClip[] keyJingleClips;
    public AudioSource audioSource;
    public AudioSource keyAudioSource;
    public int lastClipIndex1 = -1;
    public int lastClipIndex2 = -1;

    public GameObject notifPrisoner1;
    public GameObject notifPrisoner2;
    public GameObject notifExecute;
    public GameObject notifDoor;
    public GameObject notifExit;
    public NPC_Talk npcTalk;
    public Execution execution;
    public JumpscareBox jumpscareBox;
    public ExitOffice exitOffice;
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
        
        //Make sure HUD disappears when interacting with NPCs
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

        if (!audioSource.isPlaying && controller.isGrounded && moveInput.magnitude > 0.1f)
        {
            PlayStoneFootstep();
        }
        if (!keyAudioSource.isPlaying && controller.isGrounded && moveInput.magnitude > 0.1f)
        {
            PlayKeyJingle();
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
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    //Checks all of the box triggers which allow the player to interact with the mechanics of the game
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


        if (other.gameObject == notifExecute)
        {
            execution.nearExecuter = true;
        }

        if (other.gameObject == notifDoor)
        {
            jumpscareBox.inJumpscare = true;
        }

        if(other.gameObject == notifExit)
        {
            exitOffice.nearExit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var foundNpcTalk = other.GetComponentInParent<NPC_Talk>();
        if (foundNpcTalk != null)
        {
            Debug.Log("Found npc_talk");
            npcTalk = foundNpcTalk;
            npcTalk.nearNPC = false;
            return;
        }
        else
        {
            Debug.Log("Not found npc_talk");
        }

        if (other.gameObject == notifExecute)
        {
            execution.nearExecuter = false;
        }

        if (other.gameObject == notifDoor)
        {
            jumpscareBox.inJumpscare = false;
        }

        if (other.gameObject == notifExit)
        {
            exitOffice.nearExit = false;
        }
    }

    public void PlayStoneFootstep()
    {
        if (footstepStoneClips.Length == 0) return;

        int nextClipIndex;
        do
        {
            nextClipIndex = UnityEngine.Random.Range(0, footstepStoneClips.Length);
        } while (nextClipIndex == lastClipIndex1); // Avoid repeating the same sound

        lastClipIndex1 = nextClipIndex;
        audioSource.PlayOneShot(footstepStoneClips[nextClipIndex]);
    }

    public void PlayKeyJingle()
    {
        if (keyJingleClips.Length == 0) return;

        int nextClipIndex;
        do
        {
            nextClipIndex = UnityEngine.Random.Range(0, keyJingleClips.Length);
        } while (nextClipIndex == lastClipIndex2); // Avoid repeating the same sound

        lastClipIndex2 = nextClipIndex;
        keyAudioSource.PlayOneShot(keyJingleClips[nextClipIndex]);
    }
}
