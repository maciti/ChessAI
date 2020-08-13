using ChessLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIPlayerLibrary.Extensions
{
    public static class BoardExtensions
    {
        /// <summary>
        /// extension: get board representation to save it
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static char[,] TakeScreenshot(this Board board)
        {
            char[,] boardScreenshot = new char[8, 8];
            board.AvailablePieces.AsParallel().ForAll(x => boardScreenshot[x.CurrentPosition.Row, x.CurrentPosition.Column] = x.GetIdCode());
            return boardScreenshot;
        }

        /// <summary>
        /// extension: calculate the board score (value of all player pieces minus opponent pieces)
        /// </summary>
        /// <param name="board"></param>
        /// <param name="colour">Colour (White/Black)</param>
        /// <returns></returns>
        public static int CalculateScore(this Board board, PieceColour colour)
        {
            return board.AvailablePieces.Where(x => x.Colour == colour).Sum(x => x.Value)
                - board.AvailablePieces.Where(x => x.Colour != colour).Sum(x => x.Value);
        }

        /// <summary>
        /// extension: get all available moves for all the pieces of specific colour 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="playerColour"></param>
        /// <returns></returns>
        public static IEnumerable<AIMove> GetAllAvailableMoves(this Board board, PieceColour playerColour)
        {
            var opponentKingPos = board.AvailablePieces.First(x => x.Type == PieceType.King && x.Colour != playerColour).CurrentPosition;

            return board.AvailablePieces.ToList().Shuffle().Where(x => x.Colour == playerColour)
                .SelectMany(y => board.GetAvailableMoves(y)
                                .Select(z => new AIMove(y.CurrentPosition, z)))
                .Where(x => x.NewPosition.Row != opponentKingPos.Row || x.NewPosition.Column != opponentKingPos.Column)  //can't kill the king - only via checkmate
                .AsParallel();
        }

        //used for initial stages where depth level is very shallow
        private static List<Piece> Shuffle(this List<Piece> list)
        {
            Random random = new Random();

            int n = list.Count();
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Piece value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}
