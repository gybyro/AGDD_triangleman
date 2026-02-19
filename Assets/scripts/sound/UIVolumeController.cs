using System.Collections;
using UnityEngine;

public class UIVolumeController : MonoBehaviour
{
    public AudioSource source;
    public AudioClip originalClip;

    [Header("Slice Settings")]
    public float startTime = 0.22f;
    public float duration = 0.015f;
    


    public void PlayUIWhistle()
    {
        PlaySlice(startTime, duration);
    }

    public void PlaySlice(float startTime, float duration)
    {
        source.Stop();

        source.clip = originalClip;
        source.time = startTime;
        source.Play();

        StartCoroutine(StopAfter(duration));
    }

    IEnumerator StopAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        source.Stop();
    }

}