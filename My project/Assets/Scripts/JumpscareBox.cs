using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JumpscareBox : MonoBehaviour
{
    public bool inJumpscare = false;
    public bool jumpscared = false;
    public bool isLookingAtJumpscare = false;
    public Canvas jumpscareCanvas;
    public PlayerInput playerInput;
    public Animator jumpscare;
    public Image cursor;
    public Sprite cursorHand;
    public Sprite cursorReticle;
    public AudioSource audioSource;
    public AudioClip clip;

    public void Awake()
    {
        jumpscareCanvas.enabled = false;
    }
    public void Update()
    {
        if (inJumpscare)
        {
            Ray cursorRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit cursorHit;
            if (Physics.Raycast(cursorRay, out cursorHit))
            {
                if (cursorHit.collider.CompareTag("Exit"))
                {
                    isLookingAtJumpscare = true;
                    cursor.sprite = cursorHand;
                }
                else
                {
                    isLookingAtJumpscare = false;
                    cursor.sprite = cursorReticle;
                }
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isLookingAtJumpscare)
            {
                jumpscareCanvas.enabled = true;
                jumpscare.Play("Jumpscare");
                audioSource.clip = clip;
                audioSource.Play();

            }
            else
            {
                jumpscareCanvas.enabled = false;    
            }
                

        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    //The Jumpscare is a very simple png jumpscare for now, but once we have started animating and importing our models, the actual monster will jumpscare.

}
