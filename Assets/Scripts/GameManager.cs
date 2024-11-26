using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Zenject;
using EventBus = System.EventBus.EventBus;

public class GameManager : MonoBehaviour
{
    public int numberOfPlayers;
    
    [Inject] private EventBus _eventBus;
    [Inject] private CardDealer _cardDealer;
    
    private List<IGameState> _players = new List<IGameState>();
    private IGameState _currentStatePlayer;
    private int _currentPlayerIndex;
    private bool _isGameFinished;
    public void Initialize()
    {
        _eventBus.Subscribe<GameEvents.OnPlayerJoined>(OnPlayerJoined);
        _eventBus.Subscribe<GameEvents.OnPlayerTurnCompleted>(OnPlayerTurnCompleted);
        _eventBus.Subscribe<GameEvents.OnCardsFinish>(OnCardsFinish);
        _eventBus.Subscribe<GameEvents.OnGameFinish>(OnGameFinished);

        _currentPlayerIndex = 0;

        Application.targetFrameRate = 60;
    }
    private void OnDisable()
    {
        _eventBus.Unsubscribe<GameEvents.OnPlayerJoined>(OnPlayerJoined);
        _eventBus.Unsubscribe<GameEvents.OnPlayerTurnCompleted>(OnPlayerTurnCompleted);
        _eventBus.Unsubscribe<GameEvents.OnCardsFinish>(OnCardsFinish);
        _eventBus.Unsubscribe<GameEvents.OnGameFinish>(OnGameFinished);
    }
    private void OnPlayerJoined(GameEvents.OnPlayerJoined joinedEvent)
    {
       _players.Add(joinedEvent.Player);
       CheckEnoughPlayers();
    }
    private async void OnCardsFinish(GameEvents.OnCardsFinish cardsFinishEvent)
    {
        if (cardsFinishEvent.Player!=_players[0])
            return;
        
        _currentStatePlayer?.ExitState();
        await _cardDealer.DealCardsToPlayers();

        if (_isGameFinished)
            return;
        
        _currentStatePlayer = _players[_currentPlayerIndex];
        _currentStatePlayer.EnterState();
    }

    private void OnGameFinished(GameEvents.OnGameFinish gameFinishEvent)
    {
        _isGameFinished = true;
        CalculateGainedCards();

        IPlayer winner = GetWinner();
        _eventBus.Fire(new GameEvents.OnScoreCalculated(winner.PlayerName, winner.Score));
    }
    private void OnPlayerTurnCompleted(GameEvents.OnPlayerTurnCompleted turnCompletedEvent)
    {
        if (turnCompletedEvent.Player !=_currentStatePlayer)
            return;
        
        SwitchState();
    }
    private void CheckEnoughPlayers()
    {
        if (_players.Count==numberOfPlayers)
            StartGame();
    }
    private async void StartGame()
    {
        await _cardDealer.DealCardsToCenter();
        await _cardDealer.DealCardsToPlayers();
        
        _currentStatePlayer = _players[_currentPlayerIndex];
        _currentStatePlayer.EnterState();
    }
    private void SwitchState()
    {
        _currentStatePlayer?.ExitState();
        
        _currentPlayerIndex++;
        if (_currentPlayerIndex>=_players.Count)
            _currentPlayerIndex = 0;
        
        _currentStatePlayer = _players[_currentPlayerIndex];
        _currentStatePlayer.EnterState();
    }
    private void CalculateGainedCards()
    {
        _players.Sort((a, b) => ((IPlayer)b).GainedCardsCount.CompareTo(((IPlayer)a).GainedCardsCount));
        
        IPlayer firstPlayer = (IPlayer)_players[0];
        IPlayer secondPlayer = (IPlayer)_players[1];
        
        if (firstPlayer.GainedCardsCount>secondPlayer.GainedCardsCount)
            firstPlayer.AddScore(0,3);

    }
    private IPlayer GetWinner()
    {
        _players.Sort((a, b) => ((IPlayer)b).Score.CompareTo(((IPlayer)a).Score));
        
        return (IPlayer)_players[0];

    }
}
