using UnityEngine;
using UnityEngine.InputSystem;

public class JumpscareBox : MonoBehaviour
{
    public bool inJumpscare = false;
    public bool jumpscared = false;
    public Canvas doorNotif;
    public Canvas jumpscareCanvas;
    public PlayerInput playerInput;
    public Animator jumpscare;
    public void Update()
    {
        if (inJumpscare)
        {
            doorNotif.enabled = true;
        }
        else
        {
            doorNotif.enabled = false;
            jumpscareCanvas.enabled = false;
        }

    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inJumpscare)
            {
                jumpscareCanvas.enabled = true;
                jumpscare.Play("Jumpscare");
            }
            else
            {
                jumpscareCanvas.enabled = false;    
            }
                

        }
    }

}
