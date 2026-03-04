using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardSpriteData", menuName = "Data/CardSpriteData", order = 0)]
public class CardSpriteData : ScriptableObject
{
    public List<CardSprites> cardSprites = new List<CardSprites>();
    public Sprite GetCardSprite(Card card)
    {
        CardSprites cardSprite = cardSprites[card.GetSuit()];
        
        return cardSprite.cards[card.cardNumber];
    }
}
