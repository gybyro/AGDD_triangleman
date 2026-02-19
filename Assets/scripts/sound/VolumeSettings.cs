using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider uiVolumeSlider;

    void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolumeSlider.value  = PlayerPrefs.GetFloat("MusicVolume", 1f);
        uiVolumeSlider.value     = PlayerPrefs.GetFloat("UIVolume", 1f);

        SetMasterVolume();
        SetMusicVolume();
        SetUIVolume();

    }

    public void SetMasterVolume()
    {
        SetVolume(masterVolumeSlider.value, "MasterVolume");
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
    }

    public void SetMusicVolume()
    {
        SetVolume(musicVolumeSlider.value, "MusicVolume");
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
    }

    public void SetUIVolume()
    {
        SetVolume(uiVolumeSlider.value, "UIVolume");
        PlayerPrefs.SetFloat("UIVolume", uiVolumeSlider.value);
    }

    void SetVolume(float value, string parameter)
    {
        // CLAMPING     for the slider values near 0.000...
        if (value <= 0.001f)
            mixer.SetFloat(parameter, -80f);
        else
            mixer.SetFloat(parameter, Mathf.Log10(value) * 20f);   
    }
}