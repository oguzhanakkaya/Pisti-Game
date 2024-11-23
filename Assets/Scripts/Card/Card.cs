using System;
using UnityEngine;

[Serializable]
public struct Card
{
    [Header("The card suit and number. 0:S, 1:H, 2:C, 3:D")]
    public int suit;
    public int cardNumber;
    
    public Card(int suit, int cardNumber)
    {
        this.suit = suit;
        this.cardNumber = cardNumber;
    }
    public int GetSuit()
    {
        return suit;
    }
    public int GetCardNumber()
    {
        return cardNumber;
    }

}
