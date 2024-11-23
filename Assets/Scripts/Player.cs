using System.Collections;
using System.Collections.Generic;
using System.EventBus;
using Cysharp.Threading.Tasks;
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
        throw new System.NotImplementedException();
    }
}
