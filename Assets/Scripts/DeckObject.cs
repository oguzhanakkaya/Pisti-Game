using System;
using UnityEngine;

public class DeckObject : MonoBehaviour
{
    public Deck deck;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        deck = new Deck();
    }
}
