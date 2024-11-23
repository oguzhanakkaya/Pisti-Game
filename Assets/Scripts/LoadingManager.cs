using System.Collections;
using System.Collections.Generic;
using System.EventBus;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class LoadingManager : MonoInstaller
{
    [SerializeField]private DeckObject deckObject;
    [SerializeField]private PlayerInstaller playerInstaller;
    [SerializeField]private CardDealer cardDealer;
    public override void InstallBindings()
    {
        Container.Bind<EventBus>().AsSingle();
        
        deckObject.Initialize();
        cardDealer.Initialize();
        playerInstaller.Initialize();
    }
}