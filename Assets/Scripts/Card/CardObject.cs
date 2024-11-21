using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    [SerializeField]public SpriteRenderer spriteRenderer;
    [SerializeField]public TextMeshPro valueText;
    [SerializeField]public Card card;

    public void Initialize(Card card,Sprite sprite)
    {
        this.card = card;
        
        SetSprite(sprite);
        SetValueText();
    }

    private void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    private void SetValueText()
    {
        valueText.gameObject.SetActive(card.cardNumber<10);
        valueText.text = card.cardNumber.ToString();
    }
}
