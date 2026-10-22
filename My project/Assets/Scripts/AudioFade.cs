using UnityEngine;
using System.Collections;

public class AudioFade : MonoBehaviour
{
    public static IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume = .5f)
    {
        float currentTime = 0f;
        float startVolume = 0f;

        audioSource.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
