using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour,ICardObject
{
    [SerializeField]private SpriteRenderer spriteRenderer;
    [SerializeField]private BoxCollider2D boxCollider;
    [SerializeField]private Sprite unvisibleSprite;
    
    private Sprite visibleSprite;

    public bool IsVisible { get; set; }
    public Card CardData { get; set; }

    public void Initialize(Card cardData,Sprite sprite,bool isVisible)
    {
        CardData = cardData;
        IsVisible = isVisible;
        
        SetSprite(sprite);
        SetCardVisibility();
    }
    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        visibleSprite = sprite;
    }

    public void SetCardVisibility()
    {
        spriteRenderer.sprite = IsVisible ? visibleSprite : unvisibleSprite;
    }

    public void SetLayer(int layer)
    {
        spriteRenderer.sortingOrder=layer;
    }

    public async UniTask MoveCard(Vector3 position,float time)
    {
        await transform.DOMove(position, time).SetEase(Ease.Linear);
    }
}
