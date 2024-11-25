using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotPlayer : PlayerBase
{
    [SerializeField] private bool isRandom;
    
    private List<Card> playedCards=new List<Card>(); 
    private List<Card> centerCards=new List<Card>();

    public override void Initialize()
    {
        base.Initialize();
        _eventBus.Subscribe<GameEvents.OnPlayerPlayCard>(OnPlayerPlayCard);
        _eventBus.Subscribe<GameEvents.OnMakeMatch>(OnMakeMatch);
    }

    private void OnDisable()
    {
        _eventBus.Unsubscribe<GameEvents.OnPlayerPlayCard>(OnPlayerPlayCard);
        _eventBus.Unsubscribe<GameEvents.OnMakeMatch>(OnMakeMatch);
    }

    private void OnPlayerPlayCard(GameEvents.OnPlayerPlayCard evt)
    {
        centerCards.Add(evt.Card.CardData);
        playedCards.Add(evt.Card.CardData);
    }

    private void OnMakeMatch(GameEvents.OnMakeMatch evt)
    {
        centerCards.Clear();
    }
    internal override void Play()
    {
        base.Play();

        CardObject card = Cards[0];

        if (isRandom)
            card = GetRandomCard();
        else
            card = GetCardAfterCount();
        
        card.IsVisible = true;
        card.SetCardVisibility();
        PlayCard(card);
    }
    private CardObject GetRandomCard()
    {
       int i= Random.Range(0, Cards.Count);
       var card = Cards[i];
      
       return card;
    }
    private CardObject GetCardAfterCount()
    {
        if (centerCards.Count>0)
        {
            Card lastTableCard = centerCards[centerCards.Count - 1];
            
            foreach (var cardObject in Cards)
                if (cardObject.CardData.cardNumber == lastTableCard.cardNumber)
                    return cardObject;

            foreach (var cardObject in Cards)
            {
                if (cardObject.CardData.cardNumber == 10)
                    return cardObject; 
            }
               
        }
        
        foreach (var cardObject in Cards)
        {
            int countInGame = CountPlayedCards(cardObject.CardData.cardNumber);
            if (countInGame < 4)
                return cardObject;
        }
        
        CardObject randomCard = Cards[0];
        return randomCard;
    }
    private int CountPlayedCards(int value)
    {
        int count = 0;
        foreach (Card card in playedCards)
        {
            if (card.cardNumber == value)
                count++;
        }

        foreach (Card card in centerCards)
        {
            if (card.cardNumber == value)
                count++;
        }
        return count;
    }
}
