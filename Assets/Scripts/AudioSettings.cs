using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;

    void Start()
    {
        float musica = PlayerPrefs.GetFloat("MusicaVol", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVol", 1f);
        SetMusica(musica);
        SetSFX(sfx);
    }

    public void SetMusica(float vol)
    {
        mixer.SetFloat("MusicaVolume", Mathf.Log10(Mathf.Max(vol, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("MusicaVol", vol);
    }

    public void SetSFX(float vol)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(vol, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("SFXVol", vol);
    }
}