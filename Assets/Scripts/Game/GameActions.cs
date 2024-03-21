using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;

public class GameActions : MonoBehaviour
{
    [SerializeField] private GameObject _bet, _botBet, _objResult;
    [SerializeField] private GameObject _giveCard, _prefCard;
    [SerializeField] private CardInfo[] _allCards;
    [SerializeField] private CardInfo _emptyCard;

    [SerializeField] private Transform _botHand, _playerHand;
    [SerializeField] private List<int> _playedCards;

    [SerializeField] private TMP_Text _textBet, _textResult, _txtPlayerScore, _txtBotScore;

    [SerializeField] private int _playerScore, _botScore;

    [SerializeField] private GameObject _firstPanel, _secondPanel;

    [SerializeField] private AudioSource _sndGameOver, _sndWin;

    [SerializeField] private SkinInfo[] _allSkins;
    private SkinInfo _currentSkin;

    [SerializeField] private List<GameObject> _acesBot, _acesPlayer;
    private bool _gotJoker = false, _canTake = false;

    public SkinInfo GetSkin()
    {
        return _currentSkin;
    }

    private void Awake()
    {
        _currentSkin = _allSkins[PlayerPrefs.GetInt("CurrentSkin")];
    }

    private void Start()
    {
        _currentSkin = _allSkins[PlayerPrefs.GetInt("CurrentSkin")];
        PlayerPrefs.SetInt("Bet", 1);
    }

    private void Update()
    {
        _textBet.text = $"BET: {PlayerPrefs.GetInt("Bet")}C";
        _txtPlayerScore.text = _playerScore.ToString();
        _txtBotScore.text = _botScore.ToString();
    }

    private void WinGame()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + PlayerPrefs.GetInt("Bet"));
        PlayerPrefs.SetInt("Bet", 1);

        NextGame();
        _textResult.text = "WIN";
        _sndWin.Play();
        _bet.transform.DOMove(_playerHand.position, 1f);
        _botBet.transform.DOMove(_playerHand.position, 1f);
        _objResult.transform.DOLocalMoveY(0, 3f).OnStepComplete(() =>
        {
            _objResult.transform.DOLocalMoveY(-2500, 1).OnStepComplete(() => _firstPanel.SetActive(true));
            _bet.transform.position = _playerHand.transform.position;
            _botBet.transform.position = _botHand.transform.position;
        });
    }

    private void Draw()
    {
        PlayerPrefs.SetInt("Bet", 1);

        NextGame();
        _textResult.text = "DRAW";
        _sndGameOver.Play();
        _bet.transform.DOMove(_playerHand.position, 1f);
        _botBet.transform.DOMove(_botHand.position, 1f);
        _objResult.transform.DOLocalMoveY(0, 3f).OnStepComplete(()=>
        {
            _objResult.transform.DOLocalMoveY(-2500, 1).OnStepComplete(() =>_firstPanel.SetActive(true));
            _bet.transform.position = _playerHand.transform.position;
            _botBet.transform.position = _botHand.transform.position;
        });;
        
    }
    private void GameOver()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - PlayerPrefs.GetInt("Bet"));
        PlayerPrefs.SetInt("Bet", 1);
        NextGame();
        _sndGameOver.Play();
        _textResult.text = "LOSE";
        _bet.transform.DOMove(_botHand.position, 1f);
        _botBet.transform.DOMove(_botHand.position, 1f);
        _objResult.transform.DOLocalMoveY(0, 3f).OnStepComplete(()=>
        {
            _objResult.transform.DOLocalMoveY(-2500, 1).OnStepComplete(() =>_firstPanel.SetActive(true));
            _bet.transform.position = _playerHand.transform.position;
            _botBet.transform.position = _botHand.transform.position;
        });
        
    }

    private void CheckStatus()
    {
        if (_botScore > 21 && _playerScore <= 21) WinGame();
        else if (_botScore >= 21 && _playerScore >= 21) Draw();
        else if (_botScore == _playerScore) Draw();
        else if (_botScore < _playerScore && _playerScore <= 21) WinGame();
        else GameOver();
    }

    private void NextGame()
    {
        for (int i = 0; i < _botHand.childCount; i++)
            Destroy(_botHand.GetChild(i).gameObject);
        
        for (int i = 0; i < _playerHand.childCount; i++)
            Destroy(_playerHand.GetChild(i).gameObject);
        
        _playedCards.Clear();
        _botScore = 0;
        _playerScore = 0;
        _firstPanel.SetActive(false);
        _secondPanel.SetActive(false);
        _acesBot.Clear();
        _acesPlayer.Clear();
        _gotJoker = false;
        _canTake = false;
    }
    public void HangOut()
    {
        if (PlayerPrefs.GetInt("Bet") > 0)
        {
            Vector3 startPosCard = _giveCard.transform.localPosition;
            _bet.transform.DOMove(new Vector3(1,0,0), 1f);
            _botBet.transform.DOMove(new Vector3(-1,0,0), 1f);
            _giveCard.transform.DOMove(_playerHand.position, 0.5f).OnStepComplete(() =>
            {
                _giveCard.transform.localPosition = startPosCard;
                InitializePlayerCard();
                if (!_gotJoker)
                {
                    _giveCard.transform.DOMove(_botHand.position, 0.5f).OnStepComplete(() =>
                    {
                        _giveCard.transform.localPosition = startPosCard;
                        InitializeBotCard();
                        _giveCard.transform.DOMove(_playerHand.position, 0.5f).OnStepComplete(() =>
                        {
                            _giveCard.transform.localPosition = startPosCard;
                            InitializePlayerCard();
                            if (!_gotJoker)
                            {
                                _giveCard.transform.DOMove(_botHand.position, 0.5f).OnStepComplete(() =>
                                {
                                    InitializeBotCardEmpty();
                                    _giveCard.transform.localPosition = startPosCard;
                                    _canTake = true;
                                });
                            }
                        });
                    });
                }
            });
            _firstPanel.SetActive(false);
            _secondPanel.SetActive(true);
        }
    }

    private void GivePlayerCard()
    {
        _canTake = false;
        Vector3 startPosCard = _giveCard.transform.localPosition;
        _giveCard.transform.DOMove(_playerHand.position, 0.5f).OnStepComplete(() =>
            {
                InitializePlayerCard();
                _giveCard.transform.localPosition = startPosCard;
                _canTake = true;
            });
        
    }

    private void GiveBotCard()
    {
        Vector3 startPosCard = _giveCard.transform.localPosition;
        DOTween.Sequence()
            .Append(_giveCard.transform.DOMove(_botHand.position, 0.5f)).OnStepComplete(() =>
            {
                InitializeBotCard();
                _giveCard.transform.localPosition = startPosCard;
            });
    }

    private void InitializePlayerCard()
    {
        GameObject GO = Instantiate(_prefCard, _playerHand.position, quaternion.identity, _playerHand);
        GO.GetComponent<Card>().SetInfo(_allCards[ChooseCard()]);
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
        if (GO.GetComponent<Card>().GetValue() == 21)
        {
            StartCoroutine(GetJoker());
            _gotJoker = true;
            return;
        }

        if (GO.GetComponent<Card>().GetValue() == 0) _acesPlayer.Add(GO);
        _playerScore = 0;
        CalculatePlayerScore();
        if (_playerScore > 21) Pass();
    }
    private void InitializeBotCardEmpty()
    {
        GameObject GO = Instantiate(_prefCard, _botHand.position, quaternion.identity, _botHand);
        GO.GetComponent<Card>().SetInfo(_emptyCard);
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
    private void InitializeBotCard()
    {
        GameObject GO = Instantiate(_prefCard, _botHand.position, quaternion.identity, _botHand);
        GO.GetComponent<Card>().SetInfo(_allCards[ChooseCardB()]);
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
        if (GO.GetComponent<Card>().GetValue() == 0) _acesBot.Add(GO);
        _botScore = 0;
        CalculateBotScore();
        
    }

    private void CalculatePlayerScore()
    {
        for (int i = 0; i < _playerHand.childCount; i++)
        {
            _playerScore += _playerHand.GetChild(i).GetComponent<Card>().GetValue();
        }

        if (_acesPlayer.Count > 0)
        {
            for (int i = 0; i < _acesPlayer.Count; i++)
            {
                if (11 + _playerScore > 21)
                {
                    _playerScore++;
                }
                else
                {
                    _playerScore += 11;
                }
            }
        }
        
    }
    
    private void CalculateBotScore()
    {
        for (int i = 0; i < _botHand.childCount; i++)
        {
            _botScore += _botHand.GetChild(i).GetComponent<Card>().GetValue();
        }

        if (_acesBot.Count > 0)
        {
            for (int i = 0; i < _acesBot.Count; i++)
            {
                if (11 + _botScore > 21)
                {
                    _botScore++;
                }
                else
                {
                    _botScore += 11;
                }
            }
        }
        
    }

    private int RandomFromRangeWithExceptions(int rangeMin, int rangeMax, List<int> exclude)//exclude -- список чисел которые НЕ должны входить в результат
    {
        var range = Enumerable.Range(rangeMin, rangeMax).Where(i => !exclude.Contains(i));//создаем  колекцию допустимых значений
        int index = Random.Range(rangeMin, rangeMax - exclude.Count);//генерируем индекс ячейки
        return range.ElementAt(index);//возвращаем значение ячейки
    }
    private int ChooseCard()
    {
        int temp = RandomFromRangeWithExceptions(0,_allCards.Length, _playedCards);
        _playedCards.Add(temp);
        return temp;
    }
    private int ChooseCardB()
    {
        int temp = RandomFromRangeWithExceptions(0,_allCards.Length-2, _playedCards);
        _playedCards.Add(temp);
        return temp;
    }

    public void MaxBet()
    {
        PlayerPrefs.SetInt("Bet", PlayerPrefs.GetInt("Money"));
    }

    public void SetBet(int value)
    {
        if (PlayerPrefs.GetInt("Money") >= PlayerPrefs.GetInt("Bet") + value)
        {
            PlayerPrefs.SetInt("Bet", PlayerPrefs.GetInt("Bet") + value);
        }
    }

    public void ResetBet()
    {
        PlayerPrefs.SetInt("Bet", 0);
    }

    public void Pass()
    {
        if (_canTake)
        {
            _canTake = false;
            StartCoroutine(PauseWait());
        }
    }
    public void AnotherCard()
    {
        if (_canTake) 
            GivePlayerCard();
    }

    public void DoubleIt()
    {
        if (_canTake)
        {
            _canTake = false;
            Vector3 startPosCard = _giveCard.transform.localPosition;
            _giveCard.transform.DOMove(_playerHand.position, 0.5f).OnStepComplete(() =>
            {
                InitializePlayerCard();
                _giveCard.transform.localPosition = startPosCard;
            });
            if (PlayerPrefs.GetInt("Money") >= PlayerPrefs.GetInt("Bet") * 2) SetBet(PlayerPrefs.GetInt("Bet"));
            else PlayerPrefs.SetInt("Bet", PlayerPrefs.GetInt("Money"));
            StartCoroutine(PauseWait());
        }
    }

    private IEnumerator PauseWait()
    {
        if (_botHand.transform.childCount == 2)
        {
            _botHand.GetChild(1).gameObject.GetComponent<Card>().SetInfo(_allCards[ChooseCardB()]);
            _botScore += _botHand.GetChild(1).gameObject.GetComponent<Card>().GetValue();
        }
        yield return new WaitForSeconds(1.5f);
        while (_botScore < 17)
        {
            if (_botScore < 17)
                GiveBotCard();
            else
                break;
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(3f);
        CheckStatus();
    }
    
    private IEnumerator GetJoker()
    {
        yield return new WaitForSeconds(1.5f);
        WinGame();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
