using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameActions : MonoBehaviour
{
    [SerializeField] private GameObject _bet, _botBet;
    [SerializeField] private GameObject _giveCard, _prefCard;
    [SerializeField] private CardInfo[] _allCards;
    [SerializeField] private CardInfo _emptyCard;

    [SerializeField] private Transform _posPlayerCard,
                                     _posBotCard;

    [SerializeField] private Transform _botHand, _playerHand;
    [SerializeField] private List<int> _playedCards;
    
    public void HangOut()
    {
        Vector3 startPosCard = _giveCard.transform.localPosition;
        _bet.transform.DOMove(new Vector3(1,0,0), 1f);
        _botBet.transform.DOMove(new Vector3(-1,0,0), 1f);
        _giveCard.transform.DOMove(_posPlayerCard.position, 0.5f).OnStepComplete(() =>
        {
            _giveCard.transform.localPosition = startPosCard;
            InitializePlayerCard();
            _giveCard.transform.DOMove(_posBotCard.position, 0.5f).OnStepComplete(() =>
            {
                _giveCard.transform.localPosition = startPosCard;
                InitializeBotCard();
                _giveCard.transform.DOMove(_posPlayerCard.position, 0.5f).OnStepComplete(() =>
                {
                    _giveCard.transform.localPosition = startPosCard;
                    InitializePlayerCard();
                    _giveCard.transform.DOMove(_posBotCard.position, 0.5f).OnStepComplete(() =>
                    {
                        GameObject GO = Instantiate(_prefCard, _posBotCard.position, quaternion.identity, _botHand);
                        GO.GetComponent<Card>().SetInfo(_emptyCard);
                        _giveCard.transform.localPosition = startPosCard;
                    });
                });
            });
        });
    }

    private void GivePlayerCard()
    {
        Vector3 startPosCard = _giveCard.transform.localPosition;
        _giveCard.transform.DOMove(_posPlayerCard.position, 0.5f).OnStepComplete(() =>
            {
                InitializePlayerCard();
                _giveCard.transform.localPosition = startPosCard;
            });
    }

    private void GiveBotCard()
    {
        Vector3 startPosCard = _giveCard.transform.localPosition;
        DOTween.Sequence()
            .Append(_giveCard.transform.DOMove(_posBotCard.position, 0.5f)).OnStepComplete(() =>
            {
                InitializeBotCard();
                _giveCard.transform.localPosition = startPosCard;
            });
    }

    private void InitializePlayerCard()
    {
        GameObject GO = Instantiate(_prefCard, _posPlayerCard.position, quaternion.identity, _playerHand);
        int currentCard = -1;
        while (currentCard == -1)
        {
            int temp = Random.Range(0, _allCards.Length);
            if (!_playedCards.Contains(temp))
            {
                _playedCards.Add(temp);
                currentCard = temp;
            }
        }
        GO.GetComponent<Card>().SetInfo(_allCards[currentCard]);
        if (_playerHand.childCount > 1)
        {
            for (int i = 0; i < _playerHand.childCount; i++)
            {
                _playerHand.transform.GetChild(GO.transform.GetSiblingIndex()-i).transform
                    .DORotate(new Vector3(0, 0, 15*i), 1f);
                _playerHand.transform.GetChild(GO.transform.GetSiblingIndex()-i).transform
                    .DOLocalMove(i * new Vector3(-1,0,1)* 0.5f, 1f);
            }
            
        }
    }
    
    private void InitializeBotCard()
    {
        GameObject GO = Instantiate(_prefCard, _posBotCard.position, quaternion.identity, _botHand);
        GO.GetComponent<Card>().SetInfo(_allCards[Random.Range(0,_allCards.Length)]);
        if (_botHand.childCount > 1)
        {
            for (int i = 0; i < _botHand.childCount; i++)
            {
                _botHand.transform.GetChild(GO.transform.GetSiblingIndex()-i).transform
                    .DORotate(new Vector3(0, 0, 15*i), 1f);
                _botHand.transform.GetChild(GO.transform.GetSiblingIndex()-i).transform
                    .DOLocalMove(i * new Vector3(-1,0,1)* 0.5f, 1f);
            }
            
        }
    }
}
