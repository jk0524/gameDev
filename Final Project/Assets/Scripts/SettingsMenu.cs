using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public Slider slider;

    public void Start()
    {
        DontDestroyOnLoad(audioMixer);
    }

    public void SetVolume(float volume) {
        audioMixer.SetFloat("Volume", volume);
    }
}
