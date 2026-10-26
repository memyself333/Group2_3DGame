using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public Canvas pauseMenu;
    public Canvas settingsMenu;
    public float lookSensitivity;
    public float targetVolume;

    public void Start()
    {
        settingsMenu.enabled = false;
    }
    public void CloseSettings()
    {
        if (SceneManager.GetActiveScene().name == ("BackUpPrison"))
        {
            pauseMenu.enabled = true;
        }
        settingsMenu.enabled = false;
    }

    public void OnLookSensChanged(float setting)
    {
        lookSensitivity = setting;
    }

    public void OnVolumeChanged(float setting)
    {
        targetVolume = setting;
    }

}
