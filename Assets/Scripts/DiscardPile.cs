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
        
        var hasMatch = HasMatch(cardObject);
        _discardPileStack.Push(cardObject);
        
        if (player==null)
            return;

        MoveGainedCardJob gainedCardJob = new MoveGainedCardJob(_discardPileStack.ToList(),player);

        if (hasMatch)
        {
            int cardCount = _discardPileStack.Count;
            
            await gainedCardJob.ExecuteAsync();
            
            player.AddScore(cardCount,GetScore());
            
            _discardPileStack.Clear();
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

    private int GetScore()
    {
        int score = 0;

        if (_discardPileStack.Count == 2)  // Is Pisti
            score += 10;

        for (int i = 0; i < _discardPileStack.Count; i++)
        {
            var card= _discardPileStack.Pop();

            if (card.CardData.cardNumber == 0) // If Card Number 1
                score += 1;
            else if (card.CardData.cardNumber == 2 && card.CardData.suit==2) // If Card Club-2
                score += 2;
            else if (card.CardData.cardNumber == 10 && card.CardData.suit == 3) // If Card Diamond-10
                score += 3;
            else if (card.CardData.cardNumber==10) // If Card Jackpot
                score += 1;
        }
        return score;
    }
    private Vector3 GetRandomPoint()
    {
        return new Vector3(Random.Range(-offsetValue, offsetValue)+transform.position.x, 
                           Random.Range(-offsetValue, offsetValue)+transform.position.y, 
                           0);
    }
}
