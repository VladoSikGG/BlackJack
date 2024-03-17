using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoViewCoins : MonoBehaviour
{
    private void Update()
    {
        GetComponent<TMP_Text>().text = $"{PlayerPrefs.GetInt("Money")}C";
    }
}
