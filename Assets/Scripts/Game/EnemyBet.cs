using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBet : MonoBehaviour
{
    [SerializeField] private TMP_Text _textBet, _playerTextBet;
    [SerializeField] private SpriteRenderer _spritePlayer, _spriteBot;

    private void Update()
    {
        _textBet.text = _playerTextBet.text;
        _spriteBot.sprite = _spritePlayer.sprite;
    }
}
