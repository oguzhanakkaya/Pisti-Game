using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardSpriteData", menuName = "Data/CardSpriteData", order = 0)]
public class CardSpriteData : ScriptableObject
{
    public List<CardSprites> cardSprites = new List<CardSprites>();


    public Sprite GetCardSprite(Card card)
    {
        CardSprites cardSprite = cardSprites[card.GetSuit()];

        switch (card.cardNumber)
        {
            case 10:
                return cardSprite.jackCardSprites;
            case 11:
                return cardSprite.queenCardSprites;
            case 12:
                return cardSprite.kingCardSprites;
            default:
                return cardSprite.otherCardSprites;
        }
    }
    
}
