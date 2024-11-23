public class GameEvents 
{
    public struct SampleEvent
    {
        public int PlayerId { get; }
        public int Score { get; }

        public SampleEvent(int playerId, int score)
        {
            PlayerId = playerId;
            Score = score;
        }
    }
}
