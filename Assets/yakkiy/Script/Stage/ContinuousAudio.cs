using UnityEngine;

public static class ContinuousAudio 
{
    const float RANDOM_PITCH_MIN = 1f;
    const float RANDOM_PITCH_MAX = 1.5f;

    public static void PlaySoundPitchRandom(AudioSource audioSource , AudioClip clip)
    {
        audioSource.pitch = Random.Range(RANDOM_PITCH_MIN, RANDOM_PITCH_MAX);
        audioSource.PlayOneShot(clip);
    }

}
