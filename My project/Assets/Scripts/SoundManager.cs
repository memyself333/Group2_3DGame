using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource[] audioSources;
    public AudioClip[] musicClips;
    public AudioClip[] menuClips;
    public AudioClip[] objtIntClips;
    public AudioClip[] dialogueClips;
    public AudioClip[] noiseClips;
    public AudioClip[] footstepClips;
    public AudioClip[] keyClips;
    public AudioClip ambienceClip;
    public AudioClip torchClip;
    public AudioClip breathingClip;
    public AudioClip heartbeatClip;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound(AudioClip clip, int volume, AudioSource source, bool loop, bool fade)
    {
        if (!source.isPlaying)
        {
            if (loop)
            {
                source.loop = true;
            }
            else
            {
                source.loop = false;
            }
            source.volume = volume;
            source.clip = clip;
            if (fade)
            {
                StartCoroutine(AudioFade.FadeIn(source, 2f, volume));
            }
            else
            {
                source.Play();
            }
        }
    }

    public void PlayRandomSound(AudioClip[] clips, int minClip, int maxClip, float volume, AudioSource source)
    {
        if (!source.isPlaying)
        {
            int nextClipIndex;
            int lastClipIndex = -1;
            do
            {
                nextClipIndex = UnityEngine.Random.Range(minClip, maxClip);
            } while (nextClipIndex == lastClipIndex); // Avoid repeating the same sound

            lastClipIndex = nextClipIndex;
            source.volume = volume;
            source.clip = clips[nextClipIndex];
            source.Play();
        }
    }

    public void StopSound(AudioSource source, bool fade, float fadeLength = 1f)
    {
        if (fade)
        {
            StartCoroutine(AudioFade.FadeOut(source, fadeLength));
        }
        else
        {
            source.Stop();
        }
    }
}
