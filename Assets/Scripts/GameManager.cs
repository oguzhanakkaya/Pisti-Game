using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using EventBus = System.EventBus.EventBus;

public class GameManager : MonoBehaviour
{
    public int numberOfPlayers;
    
    [Inject] private EventBus _eventBus;
    [Inject] private CardDealer _cardDealer;
    
    private List<IGameState> _playersTurnStates = new List<IGameState>();

    private IGameState _currentState;
    private int _currentPlayerIndex;
    
    public void Initialize()
    {
        _eventBus.Subscribe<GameEvents.OnPlayerJoined>(OnPlayerJoined);
        _eventBus.Subscribe<GameEvents.OnPlayerTurnCompleted>(OnPlayerTurnCompleted);

        _currentPlayerIndex = 0;
    }
    private void OnDisable()
    {
        _eventBus.Unsubscribe<GameEvents.OnPlayerJoined>(OnPlayerJoined);
        _eventBus.Unsubscribe<GameEvents.OnPlayerTurnCompleted>(OnPlayerTurnCompleted);
    }
    private void OnPlayerJoined(GameEvents.OnPlayerJoined joinedEvent)
    {
       _playersTurnStates.Add(joinedEvent.Player);
       CheckEnoughPlayers();
    }
    private void OnPlayerTurnCompleted(GameEvents.OnPlayerTurnCompleted turnCompletedEvent)
    {
        if (turnCompletedEvent.Player !=_currentState)
            return;
        
        SwitchState();
    }
    private void CheckEnoughPlayers()
    {
        if (_playersTurnStates.Count==numberOfPlayers)
            StartGame();
    }
    private void StartGame()
    {
        _cardDealer.DealCardsToPlayers();
        
        _currentState = _playersTurnStates[_currentPlayerIndex];
        _currentState.EnterState();
    }
    private void SwitchState()
    {
        _currentState?.ExitState();
        
        _currentPlayerIndex++;
        if (_currentPlayerIndex>=_playersTurnStates.Count)
            _currentPlayerIndex = 0;
        
        _currentState = _playersTurnStates[_currentPlayerIndex];
        _currentState.EnterState();
    }
}
