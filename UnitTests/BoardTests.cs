using ChessLibrary;
using NUnit.Framework;

namespace UnitTests
{
    public class BoardTests
    {
        private Board _board;

        [SetUp]
        public void Setup()
        {
            _board = new Board(8);
            _board.AddAllThePieces();
        }

        [Test]
        public void TestSelectPiece()
        {
            var piece = _board.SelectPiece(0, 0); //position A8

            //I expect black rook
            Assert.IsTrue(piece.Colour == PieceColour.Black
                && piece.Type == PieceType.Rook);
        }

        [Test]
        public void TestMovePiece()
        {
            var piece = _board.SelectPiece(1, 0);  //position A7 - black pawn

            var newPosition = new Position(2, 0); // A6

            _board.MovePieceToPosition(piece, newPosition);

            Assert.IsNull(_board.SelectPiece(1, 0));
            Assert.IsNotNull(_board.SelectPiece(newPosition));
            Assert.IsTrue(piece.CurrentPosition == newPosition);
        }

        [Test]
        public void TestCheckmate()
        {
            var whitePawn = _board.SelectPiece(6, 5);
            _board.MovePieceToPosition(whitePawn, new Position(5, 5)); //F2 -> F3

            var blackPawn = _board.SelectPiece(1, 4);
            _board.MovePieceToPosition(blackPawn, new Position(3, 4)); //E7 -> E5

            var whitePawn2 = _board.SelectPiece(6, 6);
            _board.MovePieceToPosition(whitePawn2, new Position(4, 6)); //G2 -> G4

            var blackQueen = _board.SelectPiece(0, 3);
            _board.MovePieceToPosition(blackQueen, new Position(4, 7)); //D8 -> H4

            Assert.IsTrue(_board.IsCheckmated(PieceColour.White));
        }
    }
}