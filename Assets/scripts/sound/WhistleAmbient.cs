using UnityEngine;
using System.Collections;

public class WhistleAmbient : MonoBehaviour
{
    public AudioSource source;
    public AudioClip originalClip;

    [Header("Slice Settings")]
    public float startTime = 0.22f;
    public float duration = 0.015f;
    // public float duration = 0.28f;


    

  
    // void Start()
    // {
    //     // StartCoroutine(PlayAmbient());
    // }

    // float t;

    // void Update()
    // {
    //     // t += Time.deltaTime * 0.2f;
    //     // source.pitch = Mathf.Lerp(0.7f, 1.4f, Mathf.PerlinNoise(t, 0));
    // }

    // IEnumerator PlayAmbient()
    // {
    //     while(true)
    //     {
    //         source.clip = SliceClip(originalClip, 0.22f, 0.28f);
    //         source.Play();
    //         // PlaySlice(0.22f, 0.28f);
    //         // source.pitch = 0.5f;

    //         // source.pitch = notes[Random.Range(0, notes.Length)];
    //         // source.volume = Random.Range(0.15f, 0.35f);

    //         // source.PlayOneShot(source.clip);

    //         // yield return new WaitForSeconds(
    //         //     Random.Range(0.6f, 2.5f)
    //         // );


    //         // source.spatialBlend = 0f; // 2D sound
    //         // source.loop = false;
    //     }
    // }

    public void PlayWhistle()
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

    // public void PlaySlice(float start, float end)
    // {
    //     AudioClip clip = source.clip;

    //     int startSample = (int)(start * clip.frequency);
    //     int lengthSamples = (int)((end - start) * clip.frequency);

    //     source.timeSamples = startSample;
    //     source.Play();

    //     float duration = (float)lengthSamples / clip.frequency;
    //     Invoke(nameof(StopSound), duration);
    // }

    // void StopSound()
    // {
    //     source.Stop();
    // }

    // // ============= subclip
    // AudioClip SliceClip(AudioClip clip, float start, float end)
    // {
    //     start = Mathf.Clamp(start, 0f, clip.length);
    //     end   = Mathf.Clamp(end, start, clip.length);

    //     int frequency = clip.frequency;
    //     int channels = clip.channels;

    //     int startSample = Mathf.FloorToInt(start * frequency);
    //     int sampleLength = Mathf.FloorToInt((end - start) * frequency);

    //     float[] data = new float[sampleLength * channels];

    //     clip.GetData(data, startSample);

    //     AudioClip newClip = AudioClip.Create(
    //         clip.name + "_slice",
    //         sampleLength,
    //         channels,
    //         frequency,
    //         false
    //     );

    //     newClip.SetData(data, 0);
    //     return newClip;
    // }
}