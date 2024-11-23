using System;
using System.Collections;
using System.Collections.Generic;
using System.EventBus;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CardDealer : MonoBehaviour
{
    public CardSpriteData cardSpriteData;
    
    public DeckObject deckObject;
    public CardObject cardObject;
    
    private List<IPlayer> players = new List<IPlayer>();
    
    private EventBus _eventBus;

    [Inject]
    public void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    private void OnEnable()
    {
        _eventBus.Subscribe<GameEvents.OnPlayerJoined>(OnPlayerJoined);
    }

    private void OnDisable()
    {
        _eventBus.Unsubscribe<GameEvents.OnPlayerJoined>(OnPlayerJoined);
    }

    
    public void Initialize()
    {
        cardSpriteData = Resources.Load<CardSpriteData>("CardSpriteData");
    }
    
    private void OnPlayerJoined(GameEvents.OnPlayerJoined joinedEvent)
    {
        players.Add(joinedEvent.Player);
        Debug.Log("Player joined "+joinedEvent.Player);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            DealCardsToPlayers();
    }

    private async void DealCardsToPlayers()
    {
        foreach (var child in players)
        {
            for (int i = 0; i < 4; i++)
            {
                await DealCard(child is Player, child);
            }
        }
    }

    public async UniTask DealCard(bool isPlayer,IPlayer player)
    {
        Card card=deckObject.deck.GetRandomCard();
        
        var obj=Instantiate(cardObject, transform.position, Quaternion.identity);
        obj.Initialize(card,cardSpriteData.GetCardSprite(card),isPlayer);
        
        await player.TakeCard(obj);
    }
}
