using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoBehaviour
{
    [SerializeField]private User user;
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
           SpawnBot(i);
       
       SpawnUser();
    }

    private void SpawnUser()
    {
         IPlayer player1= _container.InstantiatePrefab(user, playersPoints[0], Quaternion.identity,null).GetComponent<IPlayer>();
               player1.Initialize();
    }
    private void SpawnBot(int i)
    {
        IPlayer bot= _container.InstantiatePrefab(botPlayer, playersPoints[i], Quaternion.identity,null).GetComponent<IPlayer>();
        bot.Initialize();
    }
    
    
}
