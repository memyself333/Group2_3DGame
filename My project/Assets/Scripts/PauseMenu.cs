using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Canvas pauseMenu;
    public Canvas settingsMenu;
    public PlayerInput playerInput;
    public FPController controller;
    public AudioSource musicPlayer;
    public float targetVolume = 0.5f;

    public void Awake()
    {
        pauseMenu.enabled = false;
        settingsMenu.enabled = false;
        targetVolume = 0.5f;
}
    public void Pause()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        playerInput.actions.FindAction("Movement").Disable();
        playerInput.actions.FindAction("Look").Disable();
        pauseMenu.enabled = true;
        Time.timeScale = 0;
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        playerInput.actions.FindAction("Movement").Enable();
        playerInput.actions.FindAction("Look").Enable();
        pauseMenu.enabled = false;
        Time.timeScale = 1;
    }

    public void Settings()
    {
        pauseMenu.enabled = false;
        settingsMenu.enabled = true;
    }

    public void CloseSettings()
    {
        pauseMenu.enabled = true;
        settingsMenu.enabled = false;
    }

    public void OnLookSensChanged(float setting)
    {
        controller.lookSensitivity = setting;
    }
    
    public void OnVolumeChanged(float setting)
    {
        targetVolume = setting;
        musicPlayer.volume = setting;
    }
}
