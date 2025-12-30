using System.Collections;
using UnityEngine;

public class EnvironmentalSounds : MonoBehaviour
{
    const float WAIT = 100;
    const float MIN_INTERVAL = 0.5f;

    [SerializeField] AudioClip[] zombieVoices;

    AudioSource audioSource;
   StageManager stageManager;

    float waitMin;
    float waitMax;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stageManager = StageManager.Instance;

        float value = WAIT / stageManager._Level.ZombieNum;

        int num = 1;

        if(value < MIN_INTERVAL)
        {
            num = (int)(value / MIN_INTERVAL);

            value = MIN_INTERVAL;
        }

        waitMin = value;
        waitMax = value * 2f;

        for (int i = 0; i < num; i++)
        {
            StartCoroutine(ZombieChorus());
        }
    }

    IEnumerator ZombieChorus()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(waitMin, waitMax));

            ContinuousAudio.PlaySoundPitchRandom(audioSource, zombieVoices[Random.Range(0, zombieVoices.Length - 1)]);
        }
    }
}
