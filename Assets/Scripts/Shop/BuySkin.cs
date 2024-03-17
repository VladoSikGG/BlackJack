using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySkin : MonoBehaviour
{
    [SerializeField] private int _coast;
    [SerializeField] private int _numSkin;
    [SerializeField] private GameObject _selected, _bought, _coastView;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_numSkin.ToString())) PlayerPrefs.SetInt(_numSkin.ToString(), 0);

        
        if(_numSkin == 0) PlayerPrefs.SetInt(_numSkin.ToString(), 1);
        
        if (PlayerPrefs.GetInt(_numSkin.ToString()) != 1)
        {
            _bought.SetActive(false);
            _selected.SetActive(false);
            _coastView.SetActive(true);
        }
        else if(PlayerPrefs.GetInt("CurrentSkin") != _numSkin) 
        {
            _bought.SetActive(true);
            _selected.SetActive(false);
            _coastView.SetActive(false);
        }
        else
        {
            _bought.SetActive(false);
            _selected.SetActive(true);
            _coastView.SetActive(false);
        }
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("CurrentSkin") != _numSkin && PlayerPrefs.GetInt(_numSkin.ToString()) == 1)
        {
            _bought.SetActive(true);
            _selected.SetActive(false);
        }
    }

    public void Action()
    {
        //0 - isn't bought
        Debug.Log(PlayerPrefs.GetInt("Money"));
        if (PlayerPrefs.GetInt(_numSkin.ToString()) == 0)
        {
            if (_coast <= PlayerPrefs.GetInt("Money"))
            {
                PlayerPrefs.SetInt(_numSkin.ToString(), 1);
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - _coast);
                _coastView.SetActive(false);
                _selected.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetInt("CurrentSkin", _numSkin);
            _bought.SetActive(false);
            _selected.SetActive(true);
        }
        
    }
}
