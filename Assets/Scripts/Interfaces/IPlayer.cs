using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Interfaces
{
    public interface IPlayer:IGameState
    {
        public string PlayerName { get; set; }
        Transform Transform { get; }
        public bool IsMyTurn { get; set; }
        public float MoveCardTime { get; set; }
        public float GainedCardsCount { get; set; }
        public int Score { get; set; }
        public List<CardObject> Cards { get; set; }
        public List<Transform> CardPoints { get; set; }
        public UniTask TakeCard(CardObject cards);
        public void PlayCard(CardObject card);
        public void Initialize();
        public void SendTurnCompletedEvent();
        public void AddScore(int numberOfCards,int score);

    }
}

