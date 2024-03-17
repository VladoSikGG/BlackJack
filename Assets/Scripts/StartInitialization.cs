using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInitialization : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
        
        if (!PlayerPrefs.HasKey("Money"))
        {
            PlayerPrefs.SetInt("CurrentSkin", 0);
            PlayerPrefs.SetFloat("Sounds", 100);
            PlayerPrefs.SetFloat("Music", 100);
            PlayerPrefs.SetInt("Money", 200);
            PlayerPrefs.SetInt("Year", DateTime.Now.Year);
            PlayerPrefs.SetInt("Month", DateTime.Now.Month);
            PlayerPrefs.SetInt("Day", DateTime.Now.Day);
            PlayerPrefs.SetInt("Hour", DateTime.Now.Hour);
            PlayerPrefs.SetInt("Minute", DateTime.Now.Minute);
            PlayerPrefs.SetInt("DayReward", 0);
        }
    }
}
