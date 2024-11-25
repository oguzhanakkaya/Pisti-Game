using System.EventBus;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class AppInstaller : MonoInstaller
{
    [SerializeField]private User userPrefab;
    [SerializeField]private BotPlayer botPlayerPrefab;
    [SerializeField]private GameManager gameManager;
    [SerializeField]private CardDealer cardDealer;
    [SerializeField]private DrawPile drawPile;
    [SerializeField]private DiscardPile discardPile;
    
    public override void InstallBindings()
    {
        Container.Bind<EventBus>().AsSingle();
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
        Container.Bind<CardDealer>().FromInstance(cardDealer).AsSingle();
        Container.Bind<DrawPile>().FromInstance(drawPile).AsSingle();
        Container.Bind<DiscardPile>().FromInstance(discardPile).AsSingle();
        Container.Bind<IPlayer>().FromInstance(userPrefab).AsTransient();
        Container.Bind<IPlayer>().FromInstance(botPlayerPrefab).AsTransient();
    }
}
