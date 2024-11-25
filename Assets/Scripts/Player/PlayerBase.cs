using System.Collections.Generic;
using System.EventBus;
using Cysharp.Threading.Tasks;
using Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

public class PlayerBase : MonoBehaviour,IPlayer
{
    [SerializeField] private float moveCardTime;
    [SerializeField] private List<Transform> cardPoints;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string playerName;

    public string PlayerName { get => playerName; set => playerName = value; }
    public Transform Transform { get => transform; }
    public bool IsMyTurn { get; set; }
    public float MoveCardTime { get => moveCardTime; set => moveCardTime = value; }
    
    public float GainedCardsCount { get; set; }
    public int Score { get; set; }
    public List<CardObject> Cards { get; set; }=new List<CardObject>();
    public List<Transform> CardPoints { get => cardPoints; set => cardPoints = value; }
    
    [Inject]internal EventBus _eventBus;
    
    public virtual void Initialize()
    {
        SendPlayerJoinedEvent();
        SetPlayerNameText();
        SetPlayerScoreText();
    }
    private void Update()
    {
        if (!IsMyTurn)
            return;

        if(Input.GetKeyDown(KeyCode.P))
        {
            PlayCard(null);
        }
    }
    private void SendPlayerJoinedEvent()
    {
        _eventBus.Fire(new GameEvents.OnPlayerJoined(this));
    }
    public async UniTask TakeCard(CardObject cardObject)
    {
        Cards.Add(cardObject);
        await cardObject.MoveCard(CardPoints[Cards.Count-1].position,MoveCardTime);
    }
    public void PlayCard(CardObject card)
    {
        Cards.Remove(card);
        IsMyTurn = false;
        _eventBus.Fire(new GameEvents.OnPlayerPlayCard(this,card));
        
    }
    public void SendTurnCompletedEvent()
    {
        _eventBus.Fire(new GameEvents.OnPlayerTurnCompleted(this));
    }

    public void AddScore(int numberOfCards, int score)
    {
        GainedCardsCount += numberOfCards;
        Score+=score;
        
        SetPlayerScoreText();
    }

    public void EnterState()
    {
        IsMyTurn = true;

        if (Cards.Count == 0)
        {
            _eventBus.Fire(new GameEvents.OnCardsFinish(this));
            return;
        }
        
        Play();
    }

    internal virtual void Play()
    {
        
    }
    public void ExitState()
    {
       
    }

    private void SetPlayerNameText()
    {
        nameText.text = PlayerName;
    }
    private void SetPlayerScoreText()
    {
        scoreText.text = Score.ToString();
    }
}
