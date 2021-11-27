using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public static AudioController Instance;
    
    public AudioSource Music;
    public AudioSource Barrel;
    public AudioSource Punch;

    public UnityEngine.UI.Slider MusicSlider;
    public UnityEngine.UI.Slider SoundSlider;

    public void Start()
    {
        Instance = this;
    }


    public void SetMusicVolume()
    {
        Music.volume = MusicSlider.value;
    }

    public void SetSoundVolume()
    {
        Barrel.volume = SoundSlider.value;
        Punch.volume = SoundSlider.value;
    }
}
