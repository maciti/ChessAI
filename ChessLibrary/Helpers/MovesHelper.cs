using System.Collections.Generic;
using System.Linq;

namespace ChessLibrary.Helpers
{
    public static class MovesHelper
    {
        public static IEnumerable<Position> GetPawnMoves(Piece piece, Board board)
        {
            List<Position> moves = new List<Position>();
            var currentPosition = piece.CurrentPosition;

            if (piece.Colour == PieceColour.Black) //move down
            {
                if (currentPosition.Row == board.Lenght - 1) //end of the board
                {
                    return moves;
                }

                if (board.SelectPiece(currentPosition.Row + 1, currentPosition.Column) == null) //move down
                {
                    moves.Add(new Position(currentPosition.Row + 1, currentPosition.Column));

                    if (piece.CurrentPosition.Row == 1 && board.SelectPiece(currentPosition.Row + 2, currentPosition.Column) == null)
                    {
                        moves.Add(new Position(currentPosition.Row + 2, currentPosition.Column));
                    }
                }

                if (currentPosition.Column > 0) //move down left
                {
                    var pieceOnTheWay = board.SelectPiece(currentPosition.Row + 1, currentPosition.Column - 1);

                    if (pieceOnTheWay != null && pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row + 1, currentPosition.Column - 1));
                    }
                }

                if (currentPosition.Column < board.Lenght - 1)  //move down right
                {
                    var pieceOnTheWay = board.SelectPiece(currentPosition.Row + 1, currentPosition.Column + 1);

                    if (pieceOnTheWay != null && pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row + 1, currentPosition.Column + 1));
                    }
                }
            }
            else
            {
                if (currentPosition.Row == 0)
                {
                    return moves;
                }

                if (board.SelectPiece(currentPosition.Row - 1, currentPosition.Column) == null) //move up
                {
                    moves.Add(new Position(currentPosition.Row - 1, currentPosition.Column));

                    if (piece.CurrentPosition.Row == 6 && board.SelectPiece(currentPosition.Row - 2, currentPosition.Column) == null)
                    {
                        moves.Add(new Position(currentPosition.Row - 2, currentPosition.Column));
                    }
                }

                if (currentPosition.Column > 0) //move up left
                {
                    var pieceOnTheWay = board.SelectPiece(currentPosition.Row - 1, currentPosition.Column - 1);

                    if (pieceOnTheWay != null && pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row - 1, currentPosition.Column - 1));
                    }
                }

                if (currentPosition.Column < board.Lenght - 1)  //move down right
                {
                    var pieceOnTheWay = board.SelectPiece(currentPosition.Row - 1, currentPosition.Column + 1);

                    if (pieceOnTheWay != null && pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row - 1, currentPosition.Column + 1));
                    }
                }
            }

            return moves;
        }

        public static IEnumerable<Position> GetRookMoves(Piece piece, Board board)
        {
            List<Position> moves = new List<Position>();

            int r = piece.CurrentPosition.Row;
            int c = piece.CurrentPosition.Column;
            while (r++ < board.Lenght - 1)  //move down
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }

                    break;
                }
            }

            r = piece.CurrentPosition.Row;
            while (r-- > 0) //move up
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }

                    break;
                }
            }

            r = piece.CurrentPosition.Row;
            while (c-- > 0)
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }

                    break;
                }
            }

            c = piece.CurrentPosition.Column;
            while (c++ < board.Lenght - 1)
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }

                    break;
                }
            }

            return moves;
        }


        public static IEnumerable<Position> GetKnightMoves(Piece piece, Board board)
        {
            List<Position> moves = new List<Position>();
            var currentPosition = piece.CurrentPosition;

            Piece pieceOnTheWay = null;
            //bottom 2
            if (currentPosition.Row + 2 <= board.Lenght - 1)
            {
                if (currentPosition.Column - 1 >= 0)
                {
                    pieceOnTheWay = board.SelectPiece(currentPosition.Row + 2, currentPosition.Column - 1);
                    if (pieceOnTheWay == null || pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row + 2, currentPosition.Column - 1));
                    }
                }

                if (currentPosition.Column + 1 <= board.Lenght - 1)
                {
                    pieceOnTheWay = board.SelectPiece(currentPosition.Row + 2, currentPosition.Column + 1);
                    if (pieceOnTheWay == null || pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row + 2, currentPosition.Column + 1));
                    }
                }
            }

            //top 2
            if (currentPosition.Row - 2 >= 0)
            {
                if (currentPosition.Column - 1 >= 0)
                {
                    pieceOnTheWay = board.SelectPiece(currentPosition.Row - 2, currentPosition.Column - 1);
                    if (pieceOnTheWay == null || pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row - 2, currentPosition.Column - 1));
                    }
                }

                if (currentPosition.Column + 1 <= board.Lenght - 1)
                {
                    pieceOnTheWay = board.SelectPiece(currentPosition.Row - 2, currentPosition.Column + 1);
                    if (pieceOnTheWay == null || pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row - 2, currentPosition.Column + 1));
                    }
                }
            }

            //left 2
            if (currentPosition.Column - 2 >= 0)
            {
                if (currentPosition.Row - 1 >= 0)
                {
                    pieceOnTheWay = board.SelectPiece(currentPosition.Row - 1, currentPosition.Column - 2);
                    if (pieceOnTheWay == null || pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row - 1, currentPosition.Column - 2));
                    }
                }

                if (currentPosition.Row + 1 <= board.Lenght - 1)
                {
                    pieceOnTheWay = board.SelectPiece(currentPosition.Row + 1, currentPosition.Column - 2);
                    if (pieceOnTheWay == null || pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row + 1, currentPosition.Column - 2));
                    }
                }
            }

            //right 2
            if (currentPosition.Column + 2 <= board.Lenght - 1)
            {
                if (currentPosition.Row - 1 >= 0)
                {
                    pieceOnTheWay = board.SelectPiece(currentPosition.Row - 1, currentPosition.Column + 2);
                    if (pieceOnTheWay == null || pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row - 1, currentPosition.Column + 2));
                    }
                }

                if (currentPosition.Row + 1 <= board.Lenght - 1)
                {
                    pieceOnTheWay = board.SelectPiece(currentPosition.Row + 1, currentPosition.Column + 2);
                    if (pieceOnTheWay == null || pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(currentPosition.Row + 1, currentPosition.Column + 2));
                    }
                }
            }

            return moves;
        }

        public static IEnumerable<Position> GetBishopMoves(Piece piece, Board board)
        {
            List<Position> moves = new List<Position>();

            int r = piece.CurrentPosition.Row;
            int c = piece.CurrentPosition.Column;
            while (r++ < board.Lenght - 1 && c-- > 0)  //down-left diagonal 
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }

                    break;
                }
            }

            r = piece.CurrentPosition.Row;
            c = piece.CurrentPosition.Column;
            while (r-- > 0 && c-- > 0)  //top-left diagonal 
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }

                    break;
                }
            }

            r = piece.CurrentPosition.Row;
            c = piece.CurrentPosition.Column;
            while (r++ < board.Lenght - 1 && c++ < board.Lenght - 1)  //down-right diagonal 
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }

                    break;
                }
            }

            r = piece.CurrentPosition.Row;
            c = piece.CurrentPosition.Column;
            while (r-- > 0 && c++ < board.Lenght - 1)  //top-right diagonal 
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }

                    break;
                }
            }

            return moves;
        }

        public static IEnumerable<Position> GetQueenMoves(Piece piece, Board board)
        {
            return GetRookMoves(piece, board).Union(GetBishopMoves(piece, board));
        }

        public static IEnumerable<Position> GetKingMoves(Piece piece, Board board)
        {
            List<Position> moves = new List<Position>();

            int r = piece.CurrentPosition.Row;
            int c = piece.CurrentPosition.Column;

            //up
            if (r-- > 0)
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }
                }
            }

            //down
            r = piece.CurrentPosition.Row;
            if (r++ < board.Lenght - 1)
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }
                }
            }

            r = piece.CurrentPosition.Row;
            //left
            if (c-- > 0)
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }
                }
            }

            //right
            c = piece.CurrentPosition.Column;
            if (c++ < board.Lenght - 1)
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }
                }
            }

            //top left
            r = piece.CurrentPosition.Row;
            c = piece.CurrentPosition.Column;
            if (r-- > 0 && c-- > 0)
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }
                }
            }

            //top right
            r = piece.CurrentPosition.Row;
            c = piece.CurrentPosition.Column;
            if (r-- > 0 && c++ < board.Lenght - 1)
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }
                }
            }

            //down left
            r = piece.CurrentPosition.Row;
            c = piece.CurrentPosition.Column;
            if (r++ < board.Lenght - 1 && c-- > 0)
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }
                }
            }

            //down right
            r = piece.CurrentPosition.Row;
            c = piece.CurrentPosition.Column;
            if (r++ < board.Lenght - 1 && c++ < board.Lenght - 1)
            {
                var pieceOnTheWay = board.SelectPiece(r, c);

                if (pieceOnTheWay == null)
                {
                    moves.Add(new Position(r, c));
                }
                else
                {
                    if (pieceOnTheWay.Colour != piece.Colour)
                    {
                        moves.Add(new Position(r, c));
                    }
                }
            }

            return moves;
        }

    }
}
