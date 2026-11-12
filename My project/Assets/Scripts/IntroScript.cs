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
    public bool introCurrentlyPlaying;
    public PlayerInput playerInput;
    [TextArea(3, 5)] public string[] texts;
    public PauseMenu pause;
    public SettingsMenu settings;
    public bool skipped;

    void Start()
    {
        introCurrentlyPlaying = true;
        introCanvas.enabled = true;
        introAnim.Play("IntroAnim");
        playerInput.actions.FindAction("Movement").Disable();
        playerInput.actions.FindAction("Look").Disable();
        skipped = false;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    public void PlayIntroMusic()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.musicClips[1], 1, SoundManager.instance.audioSources[0], true, true);
    }
    public void TurnOffCanvas()
    {
        if (skipped)
        {
            return;
        }
        else
        {
            introCanvas.enabled = false;
            SoundManager.instance.StopSound(SoundManager.instance.audioSources[0], true, 1f);
            SoundManager.instance.PlaySound(SoundManager.instance.ambienceClip, 1, SoundManager.instance.audioSources[4], true, true);
            playerInput.actions.FindAction("Movement").Enable();
            playerInput.actions.FindAction("Look").Enable();
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }

    public void SkipIntro()
    {
        skipped = true;
        introCurrentlyPlaying = false;
        introCanvas.enabled = false;
        SoundManager.instance.StopSound(SoundManager.instance.audioSources[0], true, 1f);
        SoundManager.instance.PlaySound(SoundManager.instance.ambienceClip, 1, SoundManager.instance.audioSources[4], true, true);
        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

}
