using System.Collections.Generic;
using System.EventBus;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Zenject;

public class CardDealer : MonoBehaviour
{
    [SerializeField]private CardObject cardObject;
    
    private List<IPlayer> _players = new List<IPlayer>();
    
    [Inject]private EventBus _eventBus;
    [Inject]private DrawPile _drawPile;
    
    private CardSpriteData _cardSpriteData;
    
    public void Initialize()
    {
        _cardSpriteData = Resources.Load<CardSpriteData>("CardSpriteData");
        
        _eventBus.Subscribe<GameEvents.OnPlayerJoined>(OnPlayerJoined);
        
    }
    
    private void OnDisable()
    {
        _eventBus.Unsubscribe<GameEvents.OnPlayerJoined>(OnPlayerJoined);
    }
    private void OnPlayerJoined(GameEvents.OnPlayerJoined joinedEvent)
    {
        _players.Add(joinedEvent.Player);
    }
    public async void DealCardsToPlayers()
    {
        await UniTask.DelayFrame(50);
        
        foreach (var child in _players)
            for (int i = 0; i < 4; i++)
                await DealCard(child is Player, child);
    }

    public async UniTask DealCard(bool isPlayer,IPlayer player)
    {
        Card card=_drawPile.deck.GetRandomCard();
        
        var obj=Instantiate(cardObject, transform.position, Quaternion.identity);
        obj.Initialize(card,_cardSpriteData.GetCardSprite(card),isPlayer);
        
        await player.TakeCard(obj);
    }
}
