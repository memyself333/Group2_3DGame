using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Canvas pauseMenu;
    public Canvas settingsMenu;
    public PlayerInput playerInput;

    public void Awake()
    {
        pauseMenu.enabled = false;
        settingsMenu.enabled = false;
}
    public void Pause()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.menuClips[3], 1, SoundManager.instance.audioSources[1], false, false);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        playerInput.actions.FindAction("Movement").Disable();
        playerInput.actions.FindAction("Look").Disable();
        pauseMenu.enabled = true;
        Time.timeScale = 0;
    }

    public void Exit()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.menuClips[3], 1, SoundManager.instance.audioSources[1], false, false);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.menuClips[3], 1, SoundManager.instance.audioSources[1], false, false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();
        pauseMenu.enabled = false;
        Time.timeScale = 1;
    }

    public void Settings()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.menuClips[3], 1, SoundManager.instance.audioSources[1], false, false);
        pauseMenu.enabled = false;
        settingsMenu.enabled = true;
    }


}
