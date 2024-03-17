using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarSounds : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.HasKey("Sounds")) GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sounds");
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("Sounds", GetComponent<Slider>().value);
    }
}
