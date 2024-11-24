using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]private DrawPile drawPile;
    [SerializeField]private DiscardPile discardPile;
    [SerializeField]private PlayerInstaller playerInstaller;
    [SerializeField]private CardDealer cardDealer;
    [SerializeField]private GameManager gameManager;
    
    public void Awake()
    {
        gameManager.Initialize();
        drawPile.Initialize();
        discardPile.Initialize();
        cardDealer.Initialize();
        playerInstaller.Initialize();
    }
}