using UnityEngine;
using System.Collections;

public class WhistleAmbient : MonoBehaviour
{
    public AudioSource source;
    public AudioClip whistleClip;

    [Header("Slice Range")]
    public float minStart = 0.2f;
    public float maxStart = 1.2f;
    public float sliceLength = 0.4f;

    [Header("Loop Section")]
    public float loopStart = 0.175f;
    public float loopEnd = 0.2f;

    [Header("Drone Settings")]
    public float basePitch = 0.35f;   // LOW = deeper drone
    public float pitchDriftAmount = 0.02f;
    public float pitchDriftSpeed = 0.15f;


    [Header("Variation")]
    public Vector2 pitchRange = new Vector2(0.9f, 1.1f);
    public Vector2 delayRange = new Vector2(0.2f, 1.2f);

    // void Start()
    // {
    //     source.clip = whistleClip;
    //     source.loop = true;
    //     source.time = loopStart;
    //     source.pitch = basePitch;
    //     source.Play();
    // }

    // void Update()
    // {
    //     // Manual loop region
    //     if (source.time >= loopEnd)
    //         source.time = loopStart;

    //     // subtle organic movement
    //     float drift =
    //         Mathf.PerlinNoise(Time.time * pitchDriftSpeed, 0f)
    //         * pitchDriftAmount;

    //     source.pitch = basePitch + drift;
    // }

    // void Update()
    // {
    //     float loopLength = loopEnd - loopStart;
    //     float t = (source.time - loopStart) / loopLength;
        
    //     // reset at the end
    //     if (source.time >= loopEnd)
    //         source.time = loopStart;

    //     // optional crossfade: fade last 20ms into start
    //     float crossfadeTime = 0.02f; // 20ms
    //     if (source.time > loopEnd - crossfadeTime)
    //     {
    //         float fadeT = (source.time - (loopEnd - crossfadeTime)) / crossfadeTime;
    //         source.volume = Mathf.Lerp(1f, 0f, fadeT);
    //     }
    //     else if (source.time < loopStart + crossfadeTime)
    //     {
    //         float fadeT = (source.time - loopStart) / crossfadeTime;
    //         source.volume = Mathf.Lerp(0f, 1f, fadeT);
    //     }
    //     else
    //     {
    //         source.volume = 1f;
    //     }
    // }


    IEnumerator MusicLoop()
    {
        while (true)
        {
            PlayRandomSlice();

            yield return new WaitForSeconds(
                sliceLength + Random.Range(delayRange.x, delayRange.y)
            );
        }
    }

    void PlayRandomSlice()
    {
        float start = Random.Range(minStart, maxStart);

        source.clip = whistleClip;
        source.time = start;

        source.pitch = Random.Range(pitchRange.x, pitchRange.y);
        source.volume = Random.Range(0.7f, 1f);

        source.Play();

        StartCoroutine(StopAfter(sliceLength));
    }

    IEnumerator StopAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        source.Stop();
    }
}


    

  
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
