using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CardObject : MonoBehaviour,ICardObject
{
    [SerializeField]public SpriteRenderer spriteRenderer;
    [SerializeField]public TextMeshPro valueText;

    public bool IsVisible { get; set; }
    public Card CardData { get; set; }

    public void Initialize(Card cardData,Sprite sprite,bool isVisible)
    {
        CardData = cardData;
        IsVisible = isVisible;
        
        SetSprite(sprite);
        SetValueText();
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
    }

    public void SetValueText()
    {
        valueText.gameObject.SetActive(CardData.cardNumber<10);
        valueText.text = CardData.cardNumber.ToString();
    }

    public async UniTask MoveCard(Vector3 position,float time)
    {
        await transform.DOMove(position, time).SetEase(Ease.Linear);
    }
}
