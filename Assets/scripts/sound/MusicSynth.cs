using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class MusicSynth : MonoBehaviour
{
    public static MusicSynth instance;
    
    const int sampleRate = 44100;

    class Note
    {
        public float frequency;
        public float duration;
    }

    List<Note> notes = new List<Note>();

    int currentNote = 0;
    double noteTime = 0;
    double phase = 0;

    float bpm = 120f;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // LoadSong("saltkjot_og_baunir");
        // AudioSettings.outputSampleRate.Equals(sampleRate);
    }
    public void Stop()
    {
        notes.Clear();
        currentNote = 0;
        noteTime = 0;
        phase = 0;
    }
    public void PlaySong(string songName)
    {
        Stop();

        LoadSong(songName);
    }

    // -------------------------
    // LOAD TEXT FILE
    // -------------------------
    public void LoadSong(string fileName)
    {
        // string path = Path.Combine(Application.streamingAssetsPath, fileName + ".txt");
        // string[] lines = File.ReadAllLines(path);
        string path = Path.Combine(
            Application.streamingAssetsPath,
            fileName + ".txt"
        );

        if (!File.Exists(path))
        {
            Debug.LogError("Song not found: " + path);
            return;
        }

        string[] lines = File.ReadAllLines(path);

        bpm = float.Parse(lines[1]);

        for (int i = 2; i < lines.Length; i++)
        {
            string[] t = lines[i].Split(' ');

            Note n = new Note();

            if (t[0] == "s")
            {
                float num = float.Parse(t[1]);
                float den = float.Parse(t[2]);

                n.frequency = 0f;
                n.duration = BeatDuration(num, den);
            }
            else
            {
                char noteName = t[0][0];
                int octave = int.Parse(t[1]);
                float num = float.Parse(t[2]);
                float den = float.Parse(t[3]);

                n.frequency = GetFrequency(noteName, octave);
                n.duration = BeatDuration(num, den);
            }

            notes.Add(n);
        }
    }

    float BeatDuration(float num, float den)
    {
        float bps = bpm / 60f;
        return (4f * num / den) / bps;
    }

    // -------------------------
    // NOTE FREQUENCIES
    // -------------------------
    float GetFrequency(char n, int oct)
    {
        float baseFreq = 440f;

        switch (n)
        {
            case 'a': baseFreq = 440; break;
            case 'A': baseFreq = 466; break;
            case 'b': baseFreq = 494; break;
            case 'c':
            case 'B': baseFreq = 523; break;
            case 'C': baseFreq = 554; break;
            case 'd': baseFreq = 587; break;
            case 'D': baseFreq = 622; break;
            case 'e': baseFreq = 659; break;
            case 'f':
            case 'E': baseFreq = 698; break;
            case 'F': baseFreq = 740; break;
            case 'g': baseFreq = 784; break;
            case 'G': baseFreq = 831; break;
        }

        return baseFreq * Mathf.Pow(2, oct - 1);
    }

    // -------------------------
    // AUDIO GENERATION
    // -------------------------
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (notes.Count == 0) return;

        for (int i = 0; i < data.Length; i += channels)
        {
            Note note = notes[currentNote];

            float sample = 0f;

            if (note.frequency > 0)
            {
                double phaseStep =
                    2.0 * Mathf.PI * note.frequency / sampleRate;

                sample = Mathf.Cos((float)phase) * 0.2f;

                phase += phaseStep;
                if (phase > Mathf.PI * 2)
                    phase -= Mathf.PI * 2;
            }

            // write sample to all channels
            for (int c = 0; c < channels; c++)
                data[i + c] = sample;

            noteTime += 1.0 / sampleRate;

            // advance note
            if (noteTime >= note.duration)
            {
                noteTime = 0;
                currentNote++;

                if (currentNote >= notes.Count)
                    currentNote = 0; // LOOP SONG
            }
        }
    }
}