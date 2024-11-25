public class User : PlayerBase
{
   public void CardPressed(CardObject card)
   {
       if (!Cards.Contains(card))
           return;
       
       PlayCard(card);
   }
}
