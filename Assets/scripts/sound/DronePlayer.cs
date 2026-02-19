using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DronePlayer : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip originalClip;   // Your whistle clip
    public float sliceStart = 0.22f; // start of the slice
    public float sliceDuration = 0.06f; // length of slice in seconds

    [Header("Drone Notes (Hz)")]
    public float[] noteFrequencies = { 110f, 130.81f, 146.83f }; // Example: A2, C3, D3
    public float noteChangeInterval = 1f; // seconds per note

    private AudioSource source;
    private AudioClip sliceClip;
    private int currentNoteIndex = 0;

    // Set the approximate frequency of your original whistle clip
    [Header("Original Clip Info")]
    public float originalFrequency = 440f; // Hz (set to main pitch of your whistle)

    // void Start()
    // {
    //     source = GetComponent<AudioSource>();

    //     // Slice the clip for looping
    //     sliceClip = SliceClip(originalClip, sliceStart, sliceStart + sliceDuration);
    //     source.clip = sliceClip;
    //     source.loop = true;
    //     source.Play();

    //     StartCoroutine(ChangeNotesRoutine());
    // }

    IEnumerator ChangeNotesRoutine()
    {
        while (true)
        {
            // Calculate pitch multiplier for the target note
            float targetFreq = noteFrequencies[currentNoteIndex];
            source.pitch = targetFreq / originalFrequency;

            // Advance to next note
            currentNoteIndex = (currentNoteIndex + 1) % noteFrequencies.Length;

            yield return new WaitForSeconds(noteChangeInterval);
        }
    }

    // ===== Slice Function =====
    AudioClip SliceClip(AudioClip clip, float start, float end)
    {
        start = Mathf.Clamp(start, 0f, clip.length);
        end = Mathf.Clamp(end, start, clip.length);

        int frequency = clip.frequency;
        int channels = clip.channels;

        int startSample = Mathf.FloorToInt(start * frequency);
        int lengthSamples = Mathf.FloorToInt((end - start) * frequency);

        float[] data = new float[lengthSamples * channels];
        clip.GetData(data, startSample);

        AudioClip newClip = AudioClip.Create(
            clip.name + "_slice",
            lengthSamples,
            channels,
            frequency,
            false
        );

        newClip.SetData(data, 0);
        return newClip;
    }
}