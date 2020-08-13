using ChessLibrary.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public class Board
    {
        //board
        private Piece[,] _piecesPositions;

        //all available pieces from the board
        public IEnumerable<Piece> AvailablePieces => _piecesPositions.Cast<Piece>().Where(x => x != null);

        //board dimension
        public int Lenght => _piecesPositions.GetLength(0);

        public Board(int lenght)
        {
            _piecesPositions = new Piece[lenght, lenght];
        }

        //used for deep copy
        public Board Clone()
        {
            var newBoard = new Board(this.Lenght);

            this.AvailablePieces.ToList().ForEach(x => { newBoard._piecesPositions[x.CurrentPosition.Row, x.CurrentPosition.Column] = x; });

            return newBoard;
        }

        /// <summary>
        /// select piece from the board
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Piece</returns>
        public Piece SelectPiece(Position position)
        {
            return _piecesPositions[position.Row, position.Column];
        }

        public Piece SelectPiece(int row, int col)
        {
            return _piecesPositions[row, col];
        }

        public void MovePieceToPosition(Piece piece, Position position)
        {
            var oldPosition = piece.CurrentPosition;

            piece.CurrentPosition = position;
            _piecesPositions[position.Row, position.Column] = piece;

            _piecesPositions[oldPosition.Row, oldPosition.Column] = null;
        }

        public bool IsCheckmated(PieceColour colour)
        {
            foreach(var piece in AvailablePieces.Where(x => x.Colour == colour))
            {
                if (GetAvailableMoves(piece).Any())
                {
                    return false; //there are still available moves -> no checkmate
                }
            }

            return true;
        }

        public IEnumerable<Position> GetAvailableMoves(Piece piece, bool checkForKingSafety = true)
        {
            IEnumerable<Position> availableMoves = null;

            switch (piece.Type)
            {
                case PieceType.Pawn:
                    availableMoves = MovesHelper.GetPawnMoves(piece, this);
                    break;
                case PieceType.Rook:
                    availableMoves = MovesHelper.GetRookMoves(piece, this);
                    break;
                case PieceType.Knight:
                    availableMoves = MovesHelper.GetKnightMoves(piece, this);
                    break;
                case PieceType.Bishop:
                    availableMoves = MovesHelper.GetBishopMoves(piece, this);
                    break;
                case PieceType.Queen:
                    availableMoves = MovesHelper.GetQueenMoves(piece, this);
                    break;
                case PieceType.King:
                    availableMoves = MovesHelper.GetKingMoves(piece, this);
                    break;

            }

            if (checkForKingSafety) //additional check to remove unsafe/illegal moves (all the moves that are exposing the king to capture, e.g. moving a pawn that is protecting the king)
            {
                List<Position> unsafeMoves = new List<Position>();

                foreach(var move in availableMoves)
                {
                    var simulationBoard = this.Clone();
                    simulationBoard.MovePieceToPosition(piece.Clone(), move);

                    Position kingPosition = simulationBoard.AvailablePieces.First(x => x.Colour == piece.Colour && x.Type == PieceType.King).CurrentPosition;

                    foreach (var opponentPiece in simulationBoard.AvailablePieces.Where(x => x.Colour != piece.Colour && (x.CurrentPosition.Row != move.Row || x.CurrentPosition.Column != move.Column)))
                    {
                        if (simulationBoard.GetAvailableMoves(opponentPiece, false).Any(x => x.Row == kingPosition.Row && x.Column == kingPosition.Column))
                        {
                            unsafeMoves.Add(move); //if a move exposes the king to the capture of an opponent piece it's an unsafe move
                        }

                    }
                }

                return availableMoves.Except(unsafeMoves);
            }

            return availableMoves;
        }

        /// <summary>
        /// add piece to the board
        /// </summary>
        /// <param name="piece"></param>
        public void AddPiece(Piece piece)
        {
            _piecesPositions[piece.CurrentPosition.Row, piece.CurrentPosition.Column] = piece;
        }

        public void AddAllThePieces()
        {
            //(row, column) (0,0) top left 

            //rook
            AddPiece(new Piece(PieceType.Rook, PieceColour.Black, 5, new Position(0, 0)));
            AddPiece(new Piece(PieceType.Rook, PieceColour.Black, 5, new Position(0, 7)));
            AddPiece(new Piece(PieceType.Rook, PieceColour.White, 5, new Position(7, 0)));
            AddPiece(new Piece(PieceType.Rook, PieceColour.White, 5, new Position(7, 7)));

            //knight
            AddPiece(new Piece(PieceType.Knight, PieceColour.Black, 3, new Position(0, 1)));
            AddPiece(new Piece(PieceType.Knight, PieceColour.Black, 3, new Position(0, 6)));
            AddPiece(new Piece(PieceType.Knight, PieceColour.White, 3, new Position(7, 1)));
            AddPiece(new Piece(PieceType.Knight, PieceColour.White, 3, new Position(7, 6)));

            //bishop
            AddPiece(new Piece(PieceType.Bishop, PieceColour.Black, 3, new Position(0, 2)));
            AddPiece(new Piece(PieceType.Bishop, PieceColour.Black, 3, new Position(0, 5)));
            AddPiece(new Piece(PieceType.Bishop, PieceColour.White, 3, new Position(7, 2)));
            AddPiece(new Piece(PieceType.Bishop, PieceColour.White, 3, new Position(7, 5)));

            //queen
            AddPiece(new Piece(PieceType.Queen, PieceColour.Black, 9, new Position(0, 3)));
            AddPiece(new Piece(PieceType.Queen, PieceColour.White, 9, new Position(7, 3)));

            //king
            AddPiece(new Piece(PieceType.King, PieceColour.Black, 1000, new Position(0, 4)));
            AddPiece(new Piece(PieceType.King, PieceColour.White, 1000, new Position(7, 4)));

            //all black pawns
            AddPiece(new Piece(PieceType.Pawn, PieceColour.Black, 1, new Position(1, 0)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.Black, 1, new Position(1, 1)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.Black, 1, new Position(1, 2)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.Black, 1, new Position(1, 3)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.Black, 1, new Position(1, 4)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.Black, 1, new Position(1, 5)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.Black, 1, new Position(1, 6)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.Black, 1, new Position(1, 7)));

            //all white pawns
            AddPiece(new Piece(PieceType.Pawn, PieceColour.White, 1, new Position(6, 0)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.White, 1, new Position(6, 1)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.White, 1, new Position(6, 2)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.White, 1, new Position(6, 3)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.White, 1, new Position(6, 4)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.White, 1, new Position(6, 5)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.White, 1, new Position(6, 6)));
            AddPiece(new Piece(PieceType.Pawn, PieceColour.White, 1, new Position(6, 7)));
        }

    }
}
