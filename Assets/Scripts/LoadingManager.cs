using System.Collections;
using System.Collections.Generic;
using System.EventBus;
using UnityEngine;
using Zenject;

public class LoadingManager : MonoInstaller
{
    [SerializeField]private DeckObject deckObject;
    public override void InstallBindings()
    {
        Container.Bind<EventBus>().AsSingle();
        
        deckObject.Initialize();
    }
}