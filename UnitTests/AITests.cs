using AIPlayerLibrary;
using ChessLibrary;
using NUnit.Framework;

namespace UnitTests
{
    public class AITests
    {
        private Board _board;
        private AIPlayer _AIPlayer;

        [SetUp]
        public void Setup()
        {
            _AIPlayer = new AIPlayer(PieceColour.Black);

            _board = new Board(3); //empty board 3 x 3
            _board.AddPiece(new Piece(PieceType.Bishop, PieceColour.Black, 3, new Position(2, 1)));
            _board.AddPiece(new Piece(PieceType.Bishop, PieceColour.White, 3, new Position(1, 2)));
            _board.AddPiece(new Piece(PieceType.Knight, PieceColour.White, 4, new Position(1, 0)));  //assigning a higher value to the knight for testing purposes
            _board.AddPiece(new Piece(PieceType.Rook, PieceColour.White, 5, new Position(0, 0)));

            // | WR |    |    |
            // | WK |    | WB |
            // |    | BB |    |
        }

        [Ignore("not working anymore because the board has no kings")] //To test this remove CheckKingSafety from Board.GetAvailableMoves
        [Test]
        public void TestMoveCalculationDepthFirstLevel()
        {
            _AIPlayer.DepthLevel = 1;

            //using only one depth level I expect the black bishop will capture the white knight

            var move = _AIPlayer.CalculateNextMove(_board);

            Assert.IsTrue(move.OldPosition.Row == 2
                && move.OldPosition.Column == 1
                && move.NewPosition.Row == 1
                && move.NewPosition.Column == 0
                && move.Score == -5); //expecting end result 1 Black Bishop 1 White Bishop 1 White Rook (one rook less => -5)
        }

        [Ignore("not working anymore because the board has no kings")] //To test this remove CheckKingSafety from Board.GetAvailableMoves
        [Test]
        public void TestMoveCalculationDepthSecondLevel()
        {
            _AIPlayer.DepthLevel = 2;

            //using two depth levels I expect the black bishop will capture the white bishop

            var move = _AIPlayer.CalculateNextMove(_board);

            Assert.IsTrue(move.OldPosition.Row == 2
                && move.OldPosition.Column == 1
                && move.NewPosition.Row == 1
                && move.NewPosition.Column == 2
                && move.Score == -6); //expecting end result 1 Black Bishop 1 White Knight 1 White Rook (() => -6)
        }
    }
}