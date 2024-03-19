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
    [SerializeField] private GameActions _gameManager;
    [SerializeField] private int numSkin;
    
    private void Start()
    {
        GetComponent<Image>().sprite = _gameManager.GetSkin().chips[transform.GetSiblingIndex()];
        _icon = GetComponent<Image>().sprite;
        _betView.sprite = _icon;
        GetComponent<Button>().onClick.AddListener(() => ButtonClick());
    }

    private void ButtonClick()
    {
        _betView.sprite = _icon;
    }
}
