using System.Collections.Generic;
using System.EventBus;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Zenject;

public class CardDealer : MonoBehaviour
{
    [SerializeField]private CardObject cardObject;
    [SerializeField]private SpriteRenderer deckSprite;
    
    private List<IPlayer> _players = new List<IPlayer>();
    
    [Inject]private EventBus _eventBus;
    [Inject]private DrawPile _drawPile;
    [Inject]private DiscardPile _discardPile;
    
    private CardSpriteData _cardSpriteData;
    private bool _isCardsFinished;
    
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
    public async UniTask DealCardsToPlayers()
    {
        await UniTask.DelayFrame(50);

        if (_isCardsFinished)
        {
            _eventBus.Fire(new GameEvents.OnGameFinish());
            await UniTask.DelayFrame(50);
            return;
        }
        
        foreach (var child in _players)
            for (int i = 0; i < 4; i++)
                await DealCard(child is Player, child);

        if (_drawPile.deck.cards.Count == 0)
        {
            _isCardsFinished = true;
            deckSprite.enabled = false;
        }
    }
    public async UniTask DealCardsToCenter()
    {
        await UniTask.DelayFrame(50);
        
        for (int i = 0; i < 3; i++)
            await DealCard(false);
        
        await DealCard(true);
           
    }

    private async UniTask DealCard(bool isPlayer,IPlayer player)
    {
        Card card=_drawPile.deck.GetRandomCard();
        
        var obj=Instantiate(cardObject, transform.position, Quaternion.identity);
        obj.Initialize(card,_cardSpriteData.GetCardSprite(card),isPlayer);
        
        await player.TakeCard(obj);
    }
    private async UniTask DealCard(bool isVisible)
    {
        Card card=_drawPile.deck.GetRandomCard();
        
        var obj=Instantiate(cardObject, transform.position, Quaternion.identity);
        obj.Initialize(card,_cardSpriteData.GetCardSprite(card),isVisible);

        await _discardPile.AddCardToDiscardPile(obj, null);
    }
}
