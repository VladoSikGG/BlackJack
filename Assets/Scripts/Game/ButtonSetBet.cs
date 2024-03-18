using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonSetBet : MonoBehaviour
{
    [SerializeField] private TMP_Text _textBet;
    [SerializeField] private SpriteRenderer _betView;
    [SerializeField] private int _value;
    private Sprite _icon;

    private void Start()
    {
        _icon = GetComponent<Image>().sprite;
        GetComponent<Button>().onClick.AddListener(() => ButtonClick(_value));
    }

    private void ButtonClick(int value)
    {
        _textBet.text = $"BET: {value}";
        _betView.sprite = _icon;
    }
}
