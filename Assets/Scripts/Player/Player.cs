using Unity.VisualScripting;
using UnityEngine;

public class Player : PlayerBase
{
   public void CardPressed(CardObject card)
   {
       if (!Cards.Contains(card))
           return;
       
       PlayCard(card);
   }
}
