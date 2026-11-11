using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public Canvas fadeCanvas;
    public Animator fade;
    public string button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeCanvas.enabled = false;
    }

    public void Awake()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.musicClips[0], 1, SoundManager.instance.audioSources[0], true, true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        button = "Start";
        fadeCanvas.enabled = true;
        fade.Play("MainMenuFade");
        SoundManager.instance.StopSound(SoundManager.instance.audioSources[0], true, 2f);
    }

    public void ExitButton()
    {
        button = "Exit";
        fadeCanvas.enabled = true;
        fade.Play("MainMenuFade");
    }

    public void AnimationEvent()
    {
        if (button == "Start")
        {
            SceneManager.LoadScene("PrisonLevel");
        }
        else if (button == "Exit")
        {
            Application.Quit();
        }
    }

}
