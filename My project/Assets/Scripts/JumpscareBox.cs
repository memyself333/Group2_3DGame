using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
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
    public PlayableDirector director;
    public Animator playerAnim;
    public Animator cameraAnim;
    public Canvas hudCanvas;
    public GameObject parentEnemy;
    public GameObject torch;
    public GameObject spotLight;
    public GameObject player;
    public GameObject door;

    public void Awake()
    {
        jumpscareCanvas.enabled = false;
        spotLight.SetActive(false);
        door.SetActive(true);
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

                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                    UnityEngine.Cursor.visible = true;

                    playerInput.actions.FindAction("Movement").Disable();
                    playerInput.actions.FindAction("Look").Disable();

                    playerAnim.enabled = true;
                    cameraAnim.enabled = true;

                    PlayExitAnimation();
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

    public void PlayExitAnimation()
    {
        SoundManager.instance.StopSound(SoundManager.instance.audioSources[0], false, 2f);
        SoundManager.instance.StopSound(SoundManager.instance.audioSources[5], false, 2f);
        hudCanvas.enabled = false;
        torch.SetActive(false);
        spotLight.SetActive(true);
        parentEnemy.transform.position = new Vector3(20.5f, -6f, 11f);
        director.Play();
    }

    public void MoveCamera()
    {
        player.transform.position = new Vector3(-9.844f, -5.212f, 5.591f);
    }
}
