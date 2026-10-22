using UnityEngine;
using System.Collections;

public class AudioFade : MonoBehaviour
{
    public static IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume = 0.5f)
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

    public static IEnumerator FadeOut(AudioSource audioSource, float duration, float targetVolume = 0f)
    {
        float currentTime = 0f;
        float startVolume = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;

        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }
}
