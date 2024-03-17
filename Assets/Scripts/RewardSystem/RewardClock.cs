using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardClock : MonoBehaviour
{
    [SerializeField] private int _timeToGet;

    [SerializeField] private int _year, _month, _day, _hour, _minutes;
    [SerializeField] private bool _canClaim;
    [SerializeField] private int _currentRewardDay;

    [SerializeField] private GameObject[] _objDayReward;
    [SerializeField] private int[] _valueReward;
    [SerializeField] private Color _claimedColor;
    
    private void OnEnable()
    {
        GetTimeInfo();
        _currentRewardDay = PlayerPrefs.GetInt("DayReward");
        //initializeView
        for (int i = 0; i < _objDayReward.Length; i++)
        {
            if (i == _currentRewardDay)
            {
                if (_canClaim)
                {
                    _objDayReward[i].transform.GetChild(0).gameObject.SetActive(false);
                    _objDayReward[i].transform.GetChild(1).gameObject.SetActive(true);
                }
                
                for (int j = 0; j < i; j++)
                {
                    Debug.Log(_objDayReward[j].transform.GetChild(0).gameObject.GetComponent<Image>().color);
                    _objDayReward[j].transform.GetChild(0).gameObject.GetComponent<Image>().color = _claimedColor;
                }

                for (int j = i; j < _objDayReward.Length; j++)
                {
                    _objDayReward[j].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
                }
            }
            else
            {
                _objDayReward[i].transform.GetChild(0).gameObject.SetActive(true);
                _objDayReward[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        
        //logic
        if (_year == DateTime.Now.Year &&
            _month == DateTime.Now.Month &&
            _day + 1 == DateTime.Now.Day && 
            _day + 2 > DateTime.Now.Day)
        {
            if (_hour < DateTime.Now.Hour)
            {
                //give reward|| 0 – not ready || 1- is ready
                _objDayReward[_currentRewardDay].transform.GetChild(0).gameObject.SetActive(false);
                _objDayReward[_currentRewardDay].transform.GetChild(1).gameObject.SetActive(true);
                _canClaim = true;
            }
            else if (_hour == DateTime.Now.Hour && _minutes <= DateTime.Now.Minute)
            {
                //giveReward
                _objDayReward[_currentRewardDay].transform.GetChild(0).gameObject.SetActive(false);
                _objDayReward[_currentRewardDay].transform.GetChild(1).gameObject.SetActive(true);
                _canClaim = true;
            }
        }
        else SetTimeInfo();
    }

    public void ClaimReward()
    {
        if (_canClaim)
        {
            int value = _valueReward[_currentRewardDay];
            _canClaim = false;
            SetTimeInfo();
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + value);
            PlayerPrefs.SetInt("DayReward", PlayerPrefs.GetInt("DayReward") + 1);
            _objDayReward[_currentRewardDay].transform.GetChild(0).gameObject.GetComponent<Image>().color = _claimedColor;
            _objDayReward[_currentRewardDay].transform.GetChild(0).gameObject.SetActive(true);
            _objDayReward[_currentRewardDay].transform.GetChild(1).gameObject.SetActive(false);
            _currentRewardDay++;
            if (_currentRewardDay == 14) _currentRewardDay = 0;
        }

    }

    private void SetTimeInfo()
    {
        _year = DateTime.Now.Year;
        _month = DateTime.Now.Month;
        _day = DateTime.Now.Day;
        _hour = DateTime.Now.Hour;
        _minutes = DateTime.Now.Minute;
        PlayerPrefs.SetInt("Year", _year);
        PlayerPrefs.SetInt("Month", _month);
        PlayerPrefs.SetInt("Day", _day);
        PlayerPrefs.SetInt("Hour", _hour);
        PlayerPrefs.SetInt("Minute", _minutes);
    }

    private void GetTimeInfo()
    {
        if (PlayerPrefs.HasKey("Year"))
        {
            _year = PlayerPrefs.GetInt("Year");
            _month = PlayerPrefs.GetInt("Month");
            _day = PlayerPrefs.GetInt("Day");
            _hour = PlayerPrefs.GetInt("Hour");
            _minutes = PlayerPrefs.GetInt("Minute");
        }
    }
}
