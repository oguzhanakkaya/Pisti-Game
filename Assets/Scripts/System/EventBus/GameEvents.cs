using Interfaces;

public class GameEvents 
{
    public struct OnPlayerJoined
    {
        public IPlayer Player;

        public OnPlayerJoined(IPlayer player)
        {
            Player = player;
        }
    }
    public struct OnPlayerTurnCompleted
    {
        public IGameState Player;

        public OnPlayerTurnCompleted(IGameState player)
        {
            Player = player;
        }
    }
}
