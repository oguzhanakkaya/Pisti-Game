using System.Collections;
using System.Collections.Generic;
using System.EventBus;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour,IPlayer
{
    [SerializeField] private float moveCardTime;
    [SerializeField] private List<Transform> cardPoints;
    
    public bool IsMyTurn { get; set; }
    public float MoveCardTime { get => moveCardTime; set => moveCardTime = value; }
    public List<CardObject> Cards { get; set; }=new List<CardObject>();
    public List<Transform> CardPoints { get => cardPoints; set => cardPoints = value; }
    
    [Inject]
    private EventBus _eventBus;
    
    public void Initialize()
    {
        SendPlayerJoinedEvent();
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
        _eventBus.Fire(new GameEvents.OnPlayerTurnCompleted(this));
    }

    public void EnterState()
    {
        Debug.LogError(gameObject.name + " is trying to enter state");
        IsMyTurn = true;
    }

    public void ExitState()
    {
        Debug.LogError(gameObject.name + " is trying to exit state");
        IsMyTurn = false;
    }
}
