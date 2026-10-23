using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public AudioSource musicPlayer;
    public Canvas fadeCanvas;
    public Animator fade;
    public AudioClip mainTheme;
    public string button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicPlayer.clip = mainTheme;
        StartCoroutine(AudioFade.FadeIn(musicPlayer, 2f, .5f));
        fadeCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        button = "Start";
        fadeCanvas.enabled = true;
        StartCoroutine(AudioFade.FadeOut(musicPlayer, 2f, 0f));
        fade.Play("MainMenuFade");
    }

    public void ExitButton()
    {
        button = "Exit";
        fadeCanvas.enabled = true;
        StartCoroutine(AudioFade.FadeOut(musicPlayer, 2f, 0f));
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
