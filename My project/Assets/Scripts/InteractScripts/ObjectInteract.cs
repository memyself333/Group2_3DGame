using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class ObjectInteract : MonoBehaviour
{
    public GameObject offset;
    public GameObject readOffset;
    public GameObject player;
    private PlayerInput playerInput;
    private GameObject targetObject;

    public bool isExamining = false;
    public bool isReading = false;
    public Marking marking;


    public Canvas objectIntCanva;
    public Canvas interactMenu;
    public GameObject readMenu;
    public Canvas hudCanvas;

    public GameObject interactBorder;

    public GameObject tableObject;

    public Animator bookAnimator;

    private Vector3 lastMousePosition;

    private Transform examinedObject; 

    private Vector3 mousePosition;

    public bool isHitting;



    private Rect screenArea = new Rect(UnityEngine.Screen.width / 2 - 600, UnityEngine.Screen.height / 2 - 375, 1200, 750);


    //List of position and rotation of the interactble objects 
    public Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
    public Dictionary<Transform, Quaternion> originalRotations = new Dictionary<Transform, Quaternion>();



    void Start()
    {
        objectIntCanva.enabled = false;
        interactMenu.enabled = false;
        readMenu.SetActive(false);
        targetObject = GameObject.Find("PlayerCapsule");
        playerInput = targetObject.GetComponent<PlayerInput>();
    }

    void Update()
    {
        // it performs a raycast from the camera to the mouse position and checks if it hits an object tagged as "Object."
        // If it does, it toggles the examination state and stores the examined object's original position and rotation.
        mousePosition = Mouse.current.position.ReadValue();
        float distance = Vector3.Distance(targetObject.transform.position, tableObject.transform.position);
        readOffset.transform.forward = -player.transform.forward;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {

            Ray interactRay = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit interactHit;


            if (Physics.Raycast(interactRay, out interactHit))
            {
                if (interactHit.collider.CompareTag("Object"))
                {
                    if (distance < 2f)
                    {
                        isExamining = true;

                        // Store the currently examined object and its original position and rotation
                        if (isExamining)
                        {
                            examinedObject = interactHit.transform;
                            originalPositions[examinedObject] = examinedObject.position;
                            originalRotations[examinedObject] = examinedObject.rotation;
                        }
                    }
                }
               
            }


        }


        //It then checks if the player is close to an interactable object using the CheckUserClose() method.
        //If the player is close, it calls either Examine() or NonExamine() and enables or disables the canvas component accordingly.
        if (CheckUserCloseToTable())
        {
            if (isExamining)
            {
                objectIntCanva.enabled = false;
                interactMenu.enabled = true;
                if (isReading)
                {
                    interactBorder.SetActive(false);
                }
                else
                {
                    interactBorder.SetActive(true);
                }
                hudCanvas.enabled = false;
                Examine(); StartExamination();
            }
            else
            {
                objectIntCanva.enabled = true;
                interactMenu.enabled = false;
                hudCanvas.enabled = true;
                NonExamine();
                
            }
        }
        else
        {
            hudCanvas.enabled = true;
            objectIntCanva.enabled = false;
        }

    }

    public void ExitButtonPressed()
    {        
        if (isReading)
        {
            readMenu.SetActive(false);
            isReading = false;
            PlayBookAnimation();
        }
        isExamining = false;
        StopExamination();
    }

    public void ReadButtonPressed()
    {
        isReading = true;
        PlayBookAnimation();
    }

    public void CloseButtonPressed()
    {
        readMenu.SetActive(false);
        isReading = false;
        PlayBookAnimation();
    }

    // This method is called when the player starts examining an object. It locks the cursor,
    // makes it visible, and disables the PlayerInput component to prevent player movement during examination.

    void StartExamination()
    {

        lastMousePosition = mousePosition;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerInput.actions.FindAction("Movement").Disable();
        playerInput.actions.FindAction("Look").Disable(); ;
    }

    //This method is called when the player stops examining an object. It locks the cursor again,
    //hides it, and re-enables the PlayerInput component to allow player movement.

    void StopExamination()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable(); ;
    }


    // This method is called when the player is examining an object.
    // It moves the examined object towards the offset object and allows the player to rotate it based on mouse movement.

    void Examine()
    {
        if (!isReading)
        {
            if (examinedObject != null)
            {
                if (screenArea.Contains(mousePosition))
                {  
                    examinedObject.position = Vector3.Lerp(examinedObject.position, offset.transform.position, 0.2f);
                    if (Mouse.current.leftButton.isPressed)
                    {
                        Vector3 deltaMouse = mousePosition - lastMousePosition;
                        float rotationSpeed = 0.5f;
                        examinedObject.Rotate(deltaMouse.x * rotationSpeed * Vector3.up, Space.World);
                        examinedObject.Rotate(deltaMouse.y * rotationSpeed * Vector3.left, Space.World);
                        lastMousePosition = mousePosition;
                    }
                }
            }
        }
    }

    //This method is called when the player is not examining an object.
    //It resets the position and rotation of the examined object to its original values stored in the dictionaries.

    void NonExamine()
    {
        if (examinedObject != null)
        {
            // Reset the position and rotation of the examined object to its original values
            if (originalPositions.ContainsKey(examinedObject))
            {
                examinedObject.position = Vector3.Lerp(examinedObject.position, originalPositions[examinedObject], 0.2f);
            }
            if (originalRotations.ContainsKey(examinedObject))
            {
                examinedObject.rotation = Quaternion.Slerp(examinedObject.rotation, originalRotations[examinedObject], 0.2f);
            }
        }
    }


    // This method calculates the distance between the player(targetObject) and 
    // an object called tableObject.If the distance is less than 2 units, it returns true, indicating that the player is close to the object.
    public bool CheckUserCloseToTable()
    {
        // Calculate the distance between the two GameObjects
        float distance = Vector3.Distance(targetObject.transform.position, tableObject.transform.position);

        // Check if they are close based on the threshold
        return (distance < 2f);

    }
    //Play Book Animations when changing between reading and examining
    public void PlayBookAnimation()
    {
        examinedObject.transform.position = readOffset.transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(player.transform.forward, Vector3.up) * Quaternion.Euler(-60f, 0f, 0f);
        if (examinedObject.name == "Letter")
        {
            examinedObject.transform.rotation = desiredRotation * Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            examinedObject.transform.rotation = desiredRotation;
        }
            
        if (isReading)
        {
            if(examinedObject.name == "Book")
            {
                bookAnimator.SetBool("CloseBook", false);
                bookAnimator.SetBool("OpenBook", true);
            }
            else 
            {
                StartCoroutine(ReadLetter());
            }
        }
        else
        {
            if (examinedObject.name == "Book")
            {
                bookAnimator.SetBool("CloseBook", true);
                bookAnimator.SetBool("OpenBook", false);
            }
            else
            {
                return;
            }
        }
    }

    //From Book script to make sure the readCanva is enabled after the animation is done
    public void ReadBook()
    {
        readMenu.SetActive(true);
    }

    public IEnumerator ReadLetter()
    {
        Debug.Log("ReadLetter Coroutine started");
        yield return new WaitForSeconds(1f);
        Debug.Log("ReadLetter Coroutine finished, enabling readMenu");
        readMenu.SetActive(true);
    }
}
