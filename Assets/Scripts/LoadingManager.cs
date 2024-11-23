using System.Collections;
using System.Collections.Generic;
using System.EventBus;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]private DeckObject deckObject;
    [SerializeField]private PlayerInstaller playerInstaller;
    [SerializeField]private CardDealer cardDealer;
    
    public void Awake()
    {
        deckObject.Initialize();
        cardDealer.Initialize();
        playerInstaller.Initialize();
    }
}