
using UnityEngine;

public class BotPlayer : PlayerBase
{
    public override void Play()
    {
        base.Play();
        PlayRandomCard();
        
    }

    public void PlayRandomCard()
    {
       int i= Random.Range(0, Cards.Count);
       var card = Cards[i];
       card.IsVisible = true;
       card.SetCardVisibility();
       PlayCard(card);
       
    }
}
