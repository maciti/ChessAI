namespace ChessLibrary
{
    public class Piece
    {
        public PieceType Type { get; }

        public PieceColour Colour { get; }

        public int Value { get; }

        public Position CurrentPosition { get; set; }

        public Piece(PieceType type, PieceColour colour, int value, Position position)
        {
            Type = type;
            Colour = colour;
            Value = value;
            CurrentPosition = position;
        }

        //deep copy
        public Piece Clone()
        {
            return new Piece(this.Type, this.Colour, this.Value, new Position(this.CurrentPosition.Row, this.CurrentPosition.Column));
        }
    }
}
