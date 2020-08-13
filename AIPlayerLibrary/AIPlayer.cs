using AIPlayerLibrary.Extensions;
using ChessLibrary;
using System;
using System.Threading.Tasks;

namespace AIPlayerLibrary
{
    public class AIPlayer
    {
        public int DepthLevel { get; set; } = 5;

        private PieceColour _AIColour;
        private PieceColour _opponentColor;

        public AIPlayer(PieceColour colour)
        {
            _AIColour = colour;
            _opponentColor = (PieceColour)(((int)_AIColour + 1) % 2);
        }

        public AIMove CalculateNextMove(Board board)
        {
            AIMove bestMove = new AIMove() { Score = int.MinValue };

            Parallel.ForEach(board.GetAllAvailableMoves(_AIColour), (move) =>
            {
                var simulationBoard = board.Clone();
                var simulationPiece = simulationBoard.SelectPiece(move.OldPosition).Clone();
                simulationBoard.MovePieceToPosition(simulationPiece, move.NewPosition);

                move.Score = MinimizeOpponentPlay(simulationBoard, level: 1, alpha: int.MinValue, beta: int.MaxValue);

                if (move.Score > bestMove.Score)
                {
                    bestMove = move;
                };
            });

            return bestMove;
        }

        //alpha: AI minimun assured score
        //beta: opponent maximun assured score
        private int MaximizeAIPlay(Board board, int level, int alpha, int beta)
        {
            if (level >= DepthLevel)
            {
                return board.CalculateScore(_AIColour);
            }

            if (board.IsCheckmated(_opponentColor))
            {
                return int.MaxValue;
            }

            if (board.IsCheckmated(_AIColour))
            {
                return int.MinValue;
            }

            int bestScore = int.MinValue;

            foreach (var move in board.GetAllAvailableMoves(_AIColour))
            {
                var simulationBoard = board.Clone();
                var simulationPiece = simulationBoard.SelectPiece(move.OldPosition).Clone();
                simulationBoard.MovePieceToPosition(simulationPiece, move.NewPosition);

                bestScore = Math.Max(bestScore, MinimizeOpponentPlay(simulationBoard, level + 1, alpha, beta));

                alpha = Math.Max(bestScore, alpha);

                if (alpha >= beta) //pruning
                {
                    break;
                }
            }

            return bestScore;
        }

        //alpha: AI minimun assured score
        //beta: opponent maximun assured score
        private int MinimizeOpponentPlay(Board board, int level, int alpha, int beta)
        {
            if (level >= DepthLevel)
            {
                return board.CalculateScore(_AIColour);
            }

            if (board.IsCheckmated(_opponentColor))
            {
                return int.MaxValue;
            }

            if (board.IsCheckmated(_AIColour))
            {
                return int.MinValue;
            }

            var bestScore = int.MaxValue;

            foreach (var move in board.GetAllAvailableMoves(_opponentColor))
            {
                var simulationBoard = board.Clone();
                var simulationPiece = simulationBoard.SelectPiece(move.OldPosition).Clone();
                simulationBoard.MovePieceToPosition(simulationPiece, move.NewPosition);

                bestScore = Math.Min(bestScore, MaximizeAIPlay(simulationBoard, level + 1, alpha, beta));

                beta = Math.Min(beta, bestScore); 

                if (beta <= alpha)//pruning
                {
                    break;
                }
            }

            return bestScore;
        }
    }
}
