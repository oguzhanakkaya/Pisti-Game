using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstaller : MonoBehaviour
{
    [SerializeField]private Player player;
    [SerializeField]private BotPlayer botPlayer;
    [SerializeField]private int numberOfPlayers;
    [SerializeField]private List<Vector3> playersPoints;
    

    public void Initialize()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
       IPlayer player1= Instantiate(player, playersPoints[0], Quaternion.identity);
       player1.Initialize();

       for (int i = 1; i < numberOfPlayers; i++)
       {
           IPlayer bot= Instantiate(botPlayer, playersPoints[0], Quaternion.identity);
           bot.Initialize();
       }
           
    }
    
    
}
