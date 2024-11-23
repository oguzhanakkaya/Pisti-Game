using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interfaces
{
    public interface ICardObject
    {
        bool IsVisible { get; set; }
        Card CardData { get; set; }
        void Initialize(Card card, Sprite sprite, bool isVisible);
        void SetSprite(Sprite sprite);
        void SetCardVisibility();
        void SetValueText();
        void MoveCard(Vector3 position,float time);
    }
}


