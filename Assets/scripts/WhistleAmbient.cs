using UnityEngine;
using System.Collections;

public class WhistleAmbient : MonoBehaviour
{
    public AudioSource source;
    public AudioClip originalClip;

    

    // calm pentatonic scale (always sounds good)
    float[] notes =
    {
        0.5f,
        0.667f,
        0.75f,
        1f,
        1.333f,
        1.5f,
        2f
    };

    void Start()
    {
        StartCoroutine(PlayAmbient());
    }

    float t;

    void Update()
    {
        t += Time.deltaTime * 0.2f;
        source.pitch = Mathf.Lerp(0.7f, 1.4f, Mathf.PerlinNoise(t, 0));
    }

    IEnumerator PlayAmbient()
    {
        while(true)
        {
            source.clip = SliceClip(originalClip, 0.22f, 0.28f);
            source.Play();
            // PlaySlice(0.22f, 0.28f);
            // source.pitch = 0.5f;

            // source.pitch = notes[Random.Range(0, notes.Length)];
            // source.volume = Random.Range(0.15f, 0.35f);

            // source.PlayOneShot(source.clip);

            // yield return new WaitForSeconds(
            //     Random.Range(0.6f, 2.5f)
            // );


            // source.spatialBlend = 0f; // 2D sound
            // source.loop = false;
        }
    }

    public void PlaySlice(float start, float end)
    {
        AudioClip clip = source.clip;

        int startSample = (int)(start * clip.frequency);
        int lengthSamples = (int)((end - start) * clip.frequency);

        source.timeSamples = startSample;
        source.Play();

        float duration = (float)lengthSamples / clip.frequency;
        Invoke(nameof(StopSound), duration);
    }

    void StopSound()
    {
        source.Stop();
    }

    // ============= subclip
    AudioClip SliceClip(AudioClip clip, float start, float end)
    {
        int frequency = clip.frequency;
        int channels = clip.channels;

        int startSample = (int)(start * frequency);
        int sampleLength = (int)((end - start) * frequency);

        float[] data = new float[sampleLength * channels];
        clip.GetData(data, startSample);

        AudioClip newClip =
            AudioClip.Create("slice",
            sampleLength,
            channels,
            frequency,
            false);

        newClip.SetData(data, 0);

        return newClip;
    }
}