using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoBehaviour
{
    [SerializeField]private Player player;
    [SerializeField]private BotPlayer botPlayer;
    [SerializeField]private List<Vector3> playersPoints;
    
    [Inject] private DiContainer _container;
    [Inject] private GameManager _gameManager;

    public void Initialize()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
       for (int i = 1; i < _gameManager.numberOfPlayers; i++)
       {
           IPlayer bot= _container.InstantiatePrefab(botPlayer, playersPoints[i], Quaternion.identity,null).GetComponent<IPlayer>();
           bot.Initialize();
       }
       IPlayer player1= _container.InstantiatePrefab(player, playersPoints[0], Quaternion.identity,null).GetComponent<IPlayer>();
       player1.Initialize();

         
    }
    
    
}
