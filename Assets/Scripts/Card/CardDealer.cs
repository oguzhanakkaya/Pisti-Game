using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardDealer : MonoBehaviour
{
    public CardSpriteData cardSpriteData;
    
    public DeckObject deckObject;
    public CardObject cardObject;
    
    private List<IPlayer> players = new List<IPlayer>();
    public Player player;
    public BotPlayer botPlayer;
    

    private void Awake()
    {
        Initialize();
        
        players.Add(player);
        players.Add(botPlayer);
    }

    private void Initialize()
    {
        cardSpriteData = Resources.Load<CardSpriteData>("CardSpriteData");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            DealCardsToPlayers();
    }

    private void DealCardsToPlayers()
    {
        foreach (var child in players)
        {
            for (int i = 0; i < 4; i++)
            {
                DealCard(child is Player, child);
            }
        }
    }

    public void DealCard(bool isPlayer,IPlayer player)
    {
        Card card=deckObject.deck.GetRandomCard();
        
        var obj=Instantiate(cardObject, transform.position, Quaternion.identity);
        obj.Initialize(card,cardSpriteData.GetCardSprite(card),isPlayer);
        
        player.TakeCard(obj);
    }
}
