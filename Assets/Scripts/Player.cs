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
    
    
    private EventBus _eventBus;

    [Inject]
    public void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
        
    }

    public void Update()
    {
        if (_eventBus != null)
            Debug.LogError("not null");
        else 
            Debug.LogError("null");

    }

    public void Initialize()
    {
     //   SendPlayerJoinedEvent();
    }
    private void SendPlayerJoinedEvent()
    {
        _eventBus.Fire(new GameEvents.OnPlayerJoined(this));
        Debug.LogError("fired");
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
