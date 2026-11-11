using System.Collections;
using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    public AudioSource soundAS;
    public AudioClip[] sounds;
    public ExitOffice exitOffice;
    public int minInt = 1;
    public int maxInt = 5;
    public int randomInt;
    public bool isRunning;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!exitOffice.isExiting)
        {
            if (!isRunning)
          
                StartCoroutine(PlayAmbientSounds());
        }
    }

    public IEnumerator PlayAmbientSounds()
    {
        isRunning = true;
        randomInt = Random.Range(minInt, maxInt);
        if (randomInt == 4)
        {
            if (!soundAS.isPlaying)
            {
                soundAS.clip = sounds[Random.Range(0, sounds.Length)];
                soundAS.Play();
            }
        }
        yield return new WaitForSeconds(5f);
        isRunning = false;
    }
}