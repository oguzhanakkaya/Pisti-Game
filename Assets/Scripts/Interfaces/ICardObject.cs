using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Interfaces
{
    public interface ICardObject
    {
        bool IsVisible { get; set; }
        Card CardData { get; set; }
        void Initialize(Card card, Sprite sprite, bool isVisible);
        void SetSprite(Sprite sprite);
        void SetCardVisibility();
        void SetValueText();
        void SetLayer(int layer);
         UniTask MoveCard(Vector3 position,float time);
    }
}


