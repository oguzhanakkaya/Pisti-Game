using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardDealer : MonoBehaviour
{
    public CardSpriteData cardSpriteData;
    
    public DeckObject deckObject;
    public CardObject cardObject;
    

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        cardSpriteData = Resources.Load<CardSpriteData>("CardSpriteData");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            DealCard();
    }

    public void DealCard()
    {
        Card card=deckObject.deck.GetRandomCard();
        
        var obj=Instantiate(cardObject, transform.position, Quaternion.identity);
        obj.Initialize(card,cardSpriteData.GetCardSprite(card));
        obj.transform.DOMove(Vector3.zero, 0.5f);
    }
}
