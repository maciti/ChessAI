using System;

namespace ChessLibrary.Exceptions
{
    public class KingInCheckException : IllegalMoveException
    {
        public KingInCheckException() : base("king is or will be in check") { }
    }
}
