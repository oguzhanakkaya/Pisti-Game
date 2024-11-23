using System.Collections;
using System.Collections.Generic;
using System.EventBus;
using Interfaces;
using UnityEngine;
using Zenject;

public class AppInstaller : MonoInstaller
{
    [SerializeField]private Player playerPrefab;
    [SerializeField]private BotPlayer botPlayerPrefab;
    [SerializeField]private GameManager gameManager;
    [SerializeField]private CardDealer cardDealer;
    
    public override void InstallBindings()
    {
        Container.Bind<EventBus>().AsSingle();
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
        Container.Bind<CardDealer>().FromInstance(cardDealer).AsSingle();
        Container.Bind<IPlayer>().FromInstance(playerPrefab).AsTransient();
        Container.Bind<IPlayer>().FromInstance(botPlayerPrefab).AsTransient();
    }
}
