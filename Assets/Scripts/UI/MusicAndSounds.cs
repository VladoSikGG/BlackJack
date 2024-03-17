using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAndSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _music, _sound;

    private void Update()
    {
        _music.volume = PlayerPrefs.GetFloat("Music");
        _sound.volume = PlayerPrefs.GetFloat("Sounds");
    }
}
