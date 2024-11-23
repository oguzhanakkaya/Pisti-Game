using System;
using UnityEngine;

public class DeckObject : MonoBehaviour
{
    public Deck deck;

    public void Initialize()
    {
        deck = new Deck();
    }
}
