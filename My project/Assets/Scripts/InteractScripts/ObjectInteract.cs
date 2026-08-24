using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
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


    public Canvas objectIntCanva;
    public Canvas interactMenu;
    public Canvas readCanva;
    public Canvas hudCanvas;

    public GameObject tableObject;

    public Animator bookAnimator;

    private Vector3 lastMousePosition;

    private Transform examinedObject; // Store the currently examined object

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
        readCanva.enabled = false;
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
                    if (distance < 1.5f)
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
                hudCanvas.enabled = false;
                Examine(); StartExamination();
            }
            else
            {
                objectIntCanva.enabled = true;
                interactMenu.enabled = false;
                hudCanvas.enabled = true;
                NonExamine(); StopExamination();
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
            readCanva.enabled = false;
            isReading = false;
            PlayBookAnimation();
        }
        isExamining = false;
    }

    public void ReadButtonPressed()
    {
        isReading = true;
        PlayBookAnimation();
    }

    public void CloseButtonPressed()
    {
        readCanva.enabled = false;
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
        playerInput.enabled = false;
    }

    //This method is called when the player stops examining an object. It locks the cursor again,
    //hides it, and re-enables the PlayerInput component to allow player movement.

    void StopExamination()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInput.enabled = true;
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
                        float rotationSpeed = 1.0f;
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
        return (distance < 1.5f);

    }
    public void PlayBookAnimation()
    {
        examinedObject.transform.position = readOffset.transform.position;
        Quaternion desiredRotation = Quaternion.LookRotation(player.transform.forward, Vector3.up) * Quaternion.Euler(-60f, 0f, 0f);
        examinedObject.transform.rotation = desiredRotation;
        if (isReading)
        {
            bookAnimator.SetBool("CloseBook", false);
            bookAnimator.SetBool("OpenBook", true);
        }
        else
        {
            bookAnimator.SetBool("OpenBook", false);
            bookAnimator.SetBool("CloseBook", true);
        }
    }

    public void ReadBook()
    {
        readCanva.enabled = true;
    }

}
