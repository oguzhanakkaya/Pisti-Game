using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IPlayer
{
    public float MoveCardTime { get; set; }
    public List<CardObject> Cards { get; set; }
    public List<Transform> CardPoints { get; set; }
    public UniTask TakeCard(CardObject cards);
    public void PlayCard(CardObject card);
    
}
