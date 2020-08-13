using System;

namespace ChessLibrary.Exceptions
{
    public class IllegalMoveException : Exception
    {
        public IllegalMoveException(string message): base($"illegal move: {message}") { }
    }
}
