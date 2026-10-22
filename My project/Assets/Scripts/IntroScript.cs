using TMPro;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    public Canvas introCanvas;
    public Animator introAnim;
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;
    [TextArea(3, 5)] public string[] texts;

    void Start()
    {
        introCanvas.enabled = true;
        introAnim.Play("IntroAnim");
        Time.timeScale = 0f;
    }

    public void TurnOffCanvas()
    {
        introCanvas.enabled = false;
        Time.timeScale = 1f;
    }
}
