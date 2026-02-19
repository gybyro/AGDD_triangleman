using System.Collections;
using UnityEngine;

public class UIVolumeController : MonoBehaviour
{
    public AudioSource source;
    public AudioClip originalClip;

    [Header("Slice Settings")]
    public float startTime = 0.22f;
    public float duration = 0.015f;
    


    // public void PlayUIWhistle()
    // {
    //     PlaySlice(startTime, duration);
    // }
    public void PlayUIWhistle()
    {
        if (source != null && source.enabled)
            source.PlayOneShot(originalClip, 0.2f); // 0.2 = volume
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