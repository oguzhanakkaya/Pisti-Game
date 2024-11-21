using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class Deck 
{
    public List<Card> cards;

    public Deck()
    {
        cards = new List<Card>();
        
        PopulateDeck();
    }
    public void PopulateDeck()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                cards.Add(new Card(i, j));
            }
        }
    }
    public Card GetRandomCard()
    {
        int rnd = Random.Range(0, cards.Count);
        
        Card card = cards[rnd];
        cards.Remove(card);
        
        return card;
    }
    public bool IsDeckEmpty()
    {
        return cards.Count == 0;
    }
}
