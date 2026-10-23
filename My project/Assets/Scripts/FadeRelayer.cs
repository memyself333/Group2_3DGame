using UnityEngine;

public class FadeRelayer : MonoBehaviour
{
    public MainMenu menu;
    public void FadeOut()
    {
        menu.AnimationEvent();
    }
}
