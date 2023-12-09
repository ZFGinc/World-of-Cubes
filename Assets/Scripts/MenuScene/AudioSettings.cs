using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [Space]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    private float _masterVolume = 0;
    private float _musicVolume = 0;
    private float _sfxVolume = 0;

    private void Start()
    {
        _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0);
        _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0);
        _sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0);

        ShowVolumeUI();
    }

    private void ShowVolumeUI()
    {
        _masterVolumeSlider.value = _masterVolume;
        _musicVolumeSlider.value = _musicVolume;
        _sfxVolumeSlider.value = _sfxVolume;
    }

    public void OnChangeMasterVolume()
    {
        _masterVolume = _masterVolumeSlider.value;
        audioMixer.SetFloat("Master", _masterVolume);
    }

    public void OnChangeMusicVolume()
    {
        _musicVolume = _musicVolumeSlider.value;
        audioMixer.SetFloat("Music", _musicVolume);
    }

    public void OnChangeSFXVolume()
    {
        _sfxVolume = _sfxVolumeSlider.value;
        audioMixer.SetFloat("SFX", _sfxVolume);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
        PlayerPrefs.SetFloat("SfxVolume", _sfxVolume);
    }

    private void OnDestroy()
    {
        Save();
    }

    private void OnDisable()
    {
        Save();
    }
}
