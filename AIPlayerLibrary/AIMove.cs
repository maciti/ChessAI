using ChessLibrary;

namespace AIPlayerLibrary
{
    public class AIMove
    {
        public Position OldPosition { get; }

        public Position NewPosition { get; }

        public int Score { get; set; }

        public AIMove() { }

        public AIMove(Position oldPosition, Position newPosition)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }
}
