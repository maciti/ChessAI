using ChessLibrary;
using System;

namespace AIPlayerLibrary.Extensions
{
    public static class PieceExtensions
    {

        /// <summary>
        /// extension: get piece code identifier
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static char GetIdCode(this Piece piece)
        {
            return Convert.ToChar((int)piece.Colour * 10 + (int)piece.Type);
        }
    }
}
