using Unity.VisualScripting;
using UnityEngine;

public class Player : PlayerBase
{
    public void Update()
    {
        if(!IsMyTurn)
            return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayCard(Cards[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayCard(Cards[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayCard(Cards[2]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayCard(Cards[3]);
        }
    }
}
