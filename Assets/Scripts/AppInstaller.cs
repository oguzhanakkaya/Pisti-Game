using System.Collections;
using System.Collections.Generic;
using System.EventBus;
using UnityEngine;
using Zenject;

public class AppInstaller : MonoInstaller
{
    [SerializeField]private Player playerPrefab;
    [SerializeField]private BotPlayer botPlayerPrefab;
    
    public override void InstallBindings()
    {
        Container.Bind<EventBus>().AsSingle();
        Container.Bind<IPlayer>().FromInstance(playerPrefab).AsTransient();
        Container.Bind<IPlayer>().FromInstance(botPlayerPrefab).AsTransient();
    }
}
