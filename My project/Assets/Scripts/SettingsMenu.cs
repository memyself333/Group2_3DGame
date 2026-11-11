using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public Canvas pauseMenu;
    public Canvas settingsMenu;
    public float lookSensitivity;
    public float targetVolume;
    [SerializeField] private AudioMixer audioMixer;

    public void Start()
    {
        settingsMenu.enabled = false;
    }
    public void CloseSettings()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.menuClips[3], 1, SoundManager.instance.audioSources[1], false, false);
        if (SceneManager.GetActiveScene().name == ("PrisonLevel"))
        {
            pauseMenu.enabled = true;
        }
        settingsMenu.enabled = false;
    }

    public void OnLookSensChanged(float setting)
    {
        lookSensitivity = setting;
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", level);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", level);
    }

    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("sfxVolume", level);
    }

    public void SetAmbienceVolume(float level)
    {
        audioMixer.SetFloat("ambienceVolume", level);
    }
}
