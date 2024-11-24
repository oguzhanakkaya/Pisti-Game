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
    public struct OnCardsFinish
    {
        public IGameState Player;

        public OnCardsFinish(IGameState player)
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
    public struct OnPlayerPlayCard
    {
        public IPlayer Player;
        public CardObject Card;

        public OnPlayerPlayCard(IPlayer player,CardObject card)
        {
            Player = player;
            Card = card;
        }
    }
    public struct OnGameFinish {}
}
