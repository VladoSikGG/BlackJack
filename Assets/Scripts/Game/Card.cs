using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardInfo _info;
    private int _value;

    public void SetInfo(CardInfo info)
    {
        _info = info;
        GetComponent<SpriteRenderer>().sprite = _info.viewSprite;
        _value = _info.value;
    }

    public int GetValue()
    {
        return _value;
    }
}
