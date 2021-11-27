using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public static AudioController Instance;
    
    public AudioSource Music;
    public AudioSource Fanfare;
    public AudioSource Barrel;
    public AudioSource Punch;
    public AudioSource Locked;
    public AudioSource ChestOpen;

    public UnityEngine.UI.Slider MusicSlider;
    public UnityEngine.UI.Slider SoundSlider;

    public void Start()
    {
        Instance = this;
    }


    public void SetMusicVolume()
    {
        Music.volume = MusicSlider.value;
        Fanfare.volume = MusicSlider.value;
    }

    public void SetSoundVolume()
    {
        Barrel.volume = SoundSlider.value;
        Punch.volume = SoundSlider.value;
        Locked.volume = SoundSlider.value;
        ChestOpen.volume = SoundSlider.value;
    }
}
