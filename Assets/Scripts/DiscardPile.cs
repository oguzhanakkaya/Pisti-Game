using System.Collections;
using System.Collections.Generic;
using System.EventBus;
using System.Linq;
using Cysharp.Threading.Tasks;
using Interfaces;
using Jobs;
using UnityEngine;
using Zenject;

public class DiscardPile : MonoBehaviour
{
    [SerializeField]private float offsetValue;
    [SerializeField]private float cardMoveSpeed;
    [SerializeField]private Stack<CardObject> _discardPileStack = new Stack<CardObject>();
    
    [Inject]private EventBus _eventBus;

    public int currentCenterScore;
    
    public void Initialize()
    {
        _eventBus.Subscribe<GameEvents.OnPlayerPlayCard>(OnPlayerPlayCard);
    }
    
    private void OnDisable()
    {
        _eventBus.Unsubscribe<GameEvents.OnPlayerPlayCard>(OnPlayerPlayCard);
    }

    private async void OnPlayerPlayCard(GameEvents.OnPlayerPlayCard obj)
    {
        await AddCardToDiscardPile(obj.Card,obj.Player);
    }
    public async UniTask AddCardToDiscardPile(CardObject cardObject,IPlayer player)
    {
        cardObject.SetLayer(_discardPileStack.Count);
        await cardObject.MoveCard(GetRandomPoint(), cardMoveSpeed);
        
        CheckCardHasScore(cardObject);
        
        var hasMatch = HasMatch(cardObject);
        _discardPileStack.Push(cardObject);
        
        if (player==null)
            return;
        
        if (hasMatch)
        {
            MoveGainedCardJob gainedCardJob = new MoveGainedCardJob(_discardPileStack.ToList(),player);
            await gainedCardJob.ExecuteAsync();
            
            player.AddScore( _discardPileStack.Count,GetScore());
            
            _discardPileStack.Clear();
            currentCenterScore = 0;
        }
        player.SendTurnCompletedEvent();
    }
    private bool HasMatch(CardObject cardObject)
    {
        if (_discardPileStack.Count==0) // If Discard Pile Empty Not Match
            return false;

        if (cardObject.CardData.cardNumber==10) // If Card is jackpot Has match
            return true;
        
        if (_discardPileStack.Peek().CardData.cardNumber== cardObject.CardData.cardNumber) // If Card Number Equal Has Match
            return true;
        
        return false;
    }

    private void CheckCardHasScore(CardObject cardObject)
    {
        if (cardObject.CardData.cardNumber == 0) // If Card Number 1
            currentCenterScore += 1;
        else if (cardObject.CardData.cardNumber == 1 && cardObject.CardData.suit==2) // If Card Club-2
            currentCenterScore += 2;
        else if (cardObject.CardData.cardNumber == 9 && cardObject.CardData.suit == 3) // If Card Diamond-10
            currentCenterScore += 3;
        else if (cardObject.CardData.cardNumber==10) // If Card Jackpot
            currentCenterScore += 1;
    }
    private int GetScore()
    {
        if (_discardPileStack.Count == 2)  // Is Pisti
            currentCenterScore += 10;
        
        return currentCenterScore;
        
    }
    private Vector3 GetRandomPoint()
    {
        return new Vector3(Random.Range(-offsetValue, offsetValue)+transform.position.x, 
                           Random.Range(-offsetValue, offsetValue)+transform.position.y, 
                           0);
    }
}
