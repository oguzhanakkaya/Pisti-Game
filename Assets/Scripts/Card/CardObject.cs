using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CardObject : MonoBehaviour,ICardObject
{
    [SerializeField]public SpriteRenderer spriteRenderer;
    [SerializeField]public BoxCollider2D boxCollider;
    [SerializeField]public TextMeshPro valueText;
    [SerializeField]public Color redColor;
    [SerializeField]public Color blackColor;
    [SerializeField]public Card cardObje;

    public bool IsVisible { get; set; }
    public Card CardData { get; set; }

    public void Initialize(Card cardData,Sprite sprite,bool isVisible)
    {
        CardData = cardData;
        IsVisible = isVisible;
        
        SetSprite(sprite);
        SetValueText();
        SetValueTextColor();
        SetCardVisibility();
    }
    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetCardVisibility()
    {
        if (!IsVisible)
            spriteRenderer.color = Color.black;
        else
            spriteRenderer.color = Color.white;
        
        valueText.gameObject.SetActive(CardData.cardNumber<10 && IsVisible);
    }
    public void SetValueText()
    {
        valueText.text = (CardData.cardNumber+1).ToString();
    }

    public void SetValueTextColor()
    {
        if (CardData.suit==0 || CardData.suit==2)
            valueText.color = blackColor;
        else 
            valueText.color = redColor;
    }

    public void SetLayer(int layer)
    {
        spriteRenderer.sortingOrder=layer;
        valueText.sortingOrder=layer;
    }

    public async UniTask MoveCard(Vector3 position,float time)
    {
        await transform.DOMove(position, time).SetEase(Ease.Linear);
    }
}
