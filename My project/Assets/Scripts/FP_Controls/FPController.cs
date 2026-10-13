using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor.Rendering;
using TMPro;
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
    public bool isSprinting = false;

    public AudioClip[] footstepStoneClips;
    public AudioClip[] keyJingleClips;
    public AudioSource audioSource;
    public AudioSource keyAudioSource;
    public int lastClipIndex1 = -1;
    public int lastClipIndex2 = -1;

    public Image cursor;
    public Sprite cursorSpeech;
    public Sprite cursorReticle;
    public bool isLookingAtNPC = false;
    public bool nearANPC = false;
    public ObjectInteract interact;

    public bool seenTalkTip;
    public bool seenExecuteTip;
    public bool seenExitTip;


    public GameObject notifPrisoner1;
    public GameObject notifPrisoner2;
    public GameObject notifExecute;
    public GameObject notifDoor;
    public GameObject notifExit;
    public NPC_Talk npcTalk;
    public Execution execution;
    public JumpscareBox jumpscareBox;
    public ExitOffice exitOffice;
    public bool isTalking;
    public DialogueManager dm;
    public Animator hintAnimator;
    public TMP_Text hintText;
    public CanvasGroup hintCanvas;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        seenExecuteTip = false;
        seenExitTip = false;
        seenTalkTip = false;
    }
    private void Update()
    {
        isTalking = dm.isDialogueActive;

        if (dm.isDialogueActive || execution.isExecuting || exitOffice.isExiting)
        {
            hintAnimator.SetBool("ShowHint", false);
            hintAnimator.SetBool("HideHint", false);
            hintAnimator.SetBool("Off", true);
        }
            if (npcTalk.nearNPC)
        {
            Debug.Log("Step1");
            Ray cursorRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit cursorHit;

            if (Physics.Raycast(cursorRay, out cursorHit))
            {
                Debug.Log("Step2");
                if (cursorHit.collider.CompareTag("NPC"))
                {
                    Debug.Log("Step3");
                    cursor.sprite = cursorSpeech;
                    isLookingAtNPC = true;
                }
                else
                {
                    cursor.sprite = cursorReticle;
                    isLookingAtNPC = false;
                }
            }
        }
        else if (execution.nearExecuter || exitOffice.nearExit || interact.CheckUserCloseToTable())
        {

        }
        else
        {
            cursor.sprite = cursorReticle;
            isLookingAtNPC = false;
        }

        HandleMovement();
        HandleLook();

        if(isSprinting)
        {
            audioSource.pitch = 1.5f; // Increase pitch for sprinting
            keyAudioSource.pitch = 1.5f; // Increase pitch for sprinting
        }
        else
        {
            audioSource.pitch = 1f; 
            keyAudioSource.pitch = 1f; 
        }

        if (isSprinting)
        {
            moveSpeed = 4f; // Sprint speed
        }
        else
        {
            moveSpeed = 2f; // Normal speed
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

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
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
            nearANPC = true;

            if (exitOffice.currentDay == 1)
            {
                if (!seenTalkTip)
                {
                    if (foundNpcTalk.thisNPC.name == "Prisoner1")
                    {
                        hintText.text = "Press [E] while facing the prisoner to talk to him.";
                        hintAnimator.SetBool("ShowHint", true);
                        hintAnimator.SetBool("HideHint", false);
                        hintAnimator.SetBool("Off", false);
                    }
                }
                else
                {
                    hintAnimator.SetBool("ShowHint", false);
                    hintAnimator.SetBool("HideHint", false);
                    hintAnimator.SetBool("Off", true);
                }
            }
        }
        else
        {
            Debug.Log("Not found npc_talk");
        }


        if (other.gameObject == notifExecute)
        {
            execution.nearExecuter = true;

            if (exitOffice.currentDay == 1)
            {
                if (execution.hasTalked)
                {
                    if (!seenExecuteTip)
                    {
                        hintText.text = "Press [E] while facing the paper to designate a prisoner to be executed.";
                        hintAnimator.SetBool("ShowHint", true);
                        hintAnimator.SetBool("HideHint", false);
                        hintAnimator.SetBool("Off", false);
                    }
                    else
                    {
                        hintAnimator.SetBool("ShowHint", false);
                        hintAnimator.SetBool("HideHint", false);
                        hintAnimator.SetBool("Off", true);
                    }
                }

            }
        }

        if (other.gameObject == notifDoor)
        {
            jumpscareBox.inJumpscare = true;
        }

        if(other.gameObject == notifExit)
        {
            exitOffice.nearExit = true;

            if (exitOffice.currentDay == 1)
            {
                if (execution.executionDone)
                {
                    if (!seenExitTip)
                    {
                        hintText.text = "Press [E] to exit your office.";
                        hintAnimator.SetBool("ShowHint", true);
                        hintAnimator.SetBool("HideHint", false);
                        hintAnimator.SetBool("Off", false);
                    }
                    else
                    {
                        hintAnimator.SetBool("ShowHint", false);
                        hintAnimator.SetBool("HideHint", false);
                        hintAnimator.SetBool("Off", true);
                    }
                }
            }
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
            nearANPC = false;

            if (exitOffice.currentDay == 1)
            {
                if (!seenTalkTip)
                {
                    hintAnimator.SetBool("ShowHint", false);
                    hintAnimator.SetBool("HideHint", true);
                    hintAnimator.SetBool("Off", false);
                }
                else
                {
                    hintAnimator.SetBool("ShowHint", false);
                    hintAnimator.SetBool("HideHint", false);
                    hintAnimator.SetBool("Off", true);
                }
            }
        }
        else
        {
            Debug.Log("Not found npc_talk");
        }

        if (other.gameObject == notifExecute)
        {
            execution.nearExecuter = false;

            if (exitOffice.currentDay == 1)
            {
                if (!seenExecuteTip)
                {
                    hintAnimator.SetBool("ShowHint", false);
                    hintAnimator.SetBool("HideHint", true);
                    hintAnimator.SetBool("Off", false);
                }
                else
                {
                    hintAnimator.SetBool("ShowHint", false);
                    hintAnimator.SetBool("HideHint", false);
                    hintAnimator.SetBool("Off", true);
                }
            }
        }

        if (other.gameObject == notifDoor)
        {
            jumpscareBox.inJumpscare = false;
        }

        if (other.gameObject == notifExit)
        {
            exitOffice.nearExit = false;

            if (exitOffice.currentDay == 1)
            {
                if (!seenExitTip)
                {
                    hintAnimator.SetBool("ShowHint", false);
                    hintAnimator.SetBool("HideHint", true);
                    hintAnimator.SetBool("Off", false);
                }
                else
                {
                    hintAnimator.SetBool("ShowHint", false);
                    hintAnimator.SetBool("HideHint", false);
                    hintAnimator.SetBool("Off", true);
                }
            }
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
