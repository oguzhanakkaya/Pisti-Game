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
}
