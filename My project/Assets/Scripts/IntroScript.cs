using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroScript : MonoBehaviour
{
    public Canvas introCanvas;
    public Animator introAnim;
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;
    public AudioSource musicPlayer;
    public AudioClip introClip;
    public bool introCurrentlyPlaying;
    public PlayerInput playerInput;
    [TextArea(3, 5)] public string[] texts;
    public PauseMenu pause;

    void Start()
    {
        introCurrentlyPlaying = true;
        introCanvas.enabled = true;
        musicPlayer.clip = introClip;
        StartCoroutine(AudioFade.FadeIn(musicPlayer, 2f, pause.targetVolume));
        introAnim.Play("IntroAnim");
        playerInput.actions.FindAction("Movement").Disable();
        playerInput.actions.FindAction("Look").Disable();
    }

    public void TurnOffCanvas()
    {
        introCanvas.enabled = false;
        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();
        StartCoroutine(AudioFade.FadeOut(musicPlayer, 2f, 0f));
    }

    public void TurnOffMusic()
    {
        musicPlayer.Stop();
        introCurrentlyPlaying = false;
    }
}
