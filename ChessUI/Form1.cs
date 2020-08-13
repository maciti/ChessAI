using AIPlayerLibrary;
using ChessLibrary;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        //private fields
        private AIPlayer _AIPlayer;
        private Board _board;
        private static Piece _previousSelectedPiece;
        private char[] _yBoardLabels = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
        private char[] _xBoardLabels = new char[] { '8', '7', '6', '5', '4', '3', '2', '1' };

        //public
        public Button[,] SquareButtons = new Button[8, 8];
        public PieceColour Turn; //indicates who has to make the next move, white or black
        public bool IsGameStarted;

        public Form1()
        {
            InitializeComponent();
            _board = new Board(8);
            _board.AddAllThePieces();
            DrawInitialBoard();
        }


        private void DrawInitialBoard()
        {
            var btnHeight = panelBoard.Height / 8;
            var btnWidht = panelBoard.Width / 8;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var button = new Button { Width = btnWidht, Height = btnHeight };
                    button.Click += SquareButton_Click;
                    button.Location = new Point(j * btnWidht, i * btnHeight);
                    button.Tag = new Position(i, j);
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.FlatAppearance.BorderColor = Color.Cyan;
                    button.BackColor = (i % 2 == 0) ? ((j % 2 == 0) ? Color.LightYellow : Color.SandyBrown) : (j % 2 == 1) ? Color.LightYellow : Color.SandyBrown;

                    var piece = _board.SelectPiece(i, j);
                    if (piece != null)
                    {
                        button.Image = Image.FromFile(@$"{AppDomain.CurrentDomain.BaseDirectory}\Icons\{piece.Colour}{piece.Type}.png");
                    }

                    SquareButtons[i, j] = button;
                    panelBoard.Controls.Add(button);
                }
            }
        }

        private void DrawPiecesOnTheBoard()
        {
            CleanBoard();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = _board.SelectPiece(i, j);
                    if (piece != null)
                    {
                        SquareButtons[i, j].Image = Image.FromFile(@$"{AppDomain.CurrentDomain.BaseDirectory}\Icons\{piece.Colour}{piece.Type}.png");
                    }
                    else
                    {
                        SquareButtons[i, j].Image = null;
                    }
                }
            }
        }

        private void SquareButton_Click(object sender, EventArgs e)
        {
            if (!IsGameStarted)
            {
                return;
            }

            if (Turn == PieceColour.Black)   //only player(white) vs AI(black). TODO player vs player 
            {
                return;
            }

            CleanBoard();

            Position newPosition = (Position)((Button)sender).Tag;

            var piece = _board.SelectPiece(newPosition);
            
            //a new piece is picked
            if (piece != null && _previousSelectedPiece != null && piece.Colour == Turn)
            {
                _previousSelectedPiece = piece;
                HighlightPieceMoves(piece);
                return;
            }

            //attempting to move to new position
            if (_previousSelectedPiece != null)
            {
                var availableMoves = _board.GetAvailableMoves(_previousSelectedPiece);

                if (!availableMoves.Any(x => x.Row == newPosition.Row && x.Column == newPosition.Column))
                {
                    _previousSelectedPiece = null;
                    return;  //illegal move
                }
                else
                {
                    //move piece
                    MovePiece(_previousSelectedPiece, newPosition);
                    _previousSelectedPiece = null;
                }
            }
            else //attempting to select a new piece
            {
                if (piece == null || piece.Colour != Turn)
                {
                    _previousSelectedPiece = null;
                    return;
                }

                _previousSelectedPiece = piece;
                HighlightPieceMoves(piece);
            }
        }

        private void MovePiece(Piece piece, Position position)
        {
            LogInfo($"{piece.Colour} {piece.Type}: {_yBoardLabels[piece.CurrentPosition.Column]}{_xBoardLabels[piece.CurrentPosition.Row]} -> {_yBoardLabels[position.Column]}{_xBoardLabels[position.Row]}");

            RedrawImageIntoNewPosition(pieceWithOldPosition: _previousSelectedPiece, newPosition: position);
            _board.MovePieceToPosition(piece, position);

            if (_board.IsCheckmated(PieceColour.Black)) //check for checkmate after the move
            {
                MessageBox.Show("Checkmate");
            }

            Turn = PieceColour.Black; //black turn
            panelTurn.BackColor = Color.Black;

            LogInfo($"{Turn} Turn");
        }

        //generate AI next move
        private void AINextMove()
        {
            AIMove move = _AIPlayer.CalculateNextMove(_board);

            var pieceToBeMoved = _board.SelectPiece(move.OldPosition);

            LogInfo($"{pieceToBeMoved.Colour} {pieceToBeMoved.Type}: {_yBoardLabels[pieceToBeMoved.CurrentPosition.Column]}{_xBoardLabels[pieceToBeMoved.CurrentPosition.Row]} -> {_yBoardLabels[move.NewPosition.Column]}{_xBoardLabels[move.NewPosition.Row]}");

            RedrawImageIntoNewPosition(pieceWithOldPosition: pieceToBeMoved, newPosition: move.NewPosition);

            _board.MovePieceToPosition(pieceToBeMoved, move.NewPosition);

            if (_board.IsCheckmated(PieceColour.White)) //check for checkmate after the move
            {
                MessageBox.Show("Checkmate");
            }

            Turn = PieceColour.White;  //white turn
            panelTurn.BackColor = Color.White;

            LogInfo($"{Turn} Turn");
        }

        /// <summary>
        /// Hightlights squares on which the piece can move
        /// </summary>
        /// <param name="piece"></param>
        private void HighlightPieceMoves(Piece piece)
        {
            //highlight square
            SquareButtons[piece.CurrentPosition.Row, piece.CurrentPosition.Column].FlatAppearance.BorderSize = 2;

            //highlight available squares
            _board.GetAvailableMoves(piece).AsParallel().ForAll(move =>
            {
                var currentColor = SquareButtons[move.Row, move.Column].BackColor;
                SquareButtons[move.Row, move.Column].BackColor = Color.FromArgb(80, currentColor.R, currentColor.G, currentColor.B);
            });
        }

        /// <summary>
        /// remove highligths and selections
        /// </summary>
        private void CleanBoard()
        {
            panelBoard.Controls.OfType<Button>().AsParallel().ForAll(button =>
            {
                button.FlatAppearance.BorderSize = 0;
                var p = (Position)button.Tag;
                button.BackColor = (p.Row % 2 == 0) ? ((p.Column % 2 == 0) ? Color.LightYellow : Color.SandyBrown) : (p.Column % 2 == 1) ? Color.LightYellow : Color.SandyBrown;
            });
        }

        //moving the piece icon from one button to another
        private void RedrawImageIntoNewPosition(Piece pieceWithOldPosition, Position newPosition)
        {
            SquareButtons[pieceWithOldPosition.CurrentPosition.Row, pieceWithOldPosition.CurrentPosition.Column].Image = null;
            SquareButtons[newPosition.Row, newPosition.Column].Image = Image.FromFile(@$"{AppDomain.CurrentDomain.BaseDirectory}\Icons\{pieceWithOldPosition.Colour}{pieceWithOldPosition.Type}.png");

        }

        private void startGame_Click(object sender, EventArgs e)
        {
            //creating AI player with depth level from textBoxAILevel.Text
            _AIPlayer = new AIPlayer(PieceColour.Black);
            _AIPlayer.DepthLevel = Convert.ToInt32(textBoxAILevel.Text);
            
            //creating classic board
            _board = new Board(8);
            _board.AddAllThePieces();
  
            DrawPiecesOnTheBoard();
            Turn = PieceColour.White;
            panelTurn.BackColor = Color.White;
            IsGameStarted = true;

            LogInfo($"New Game Started! {Turn} Turn");
        }

        private static bool _isAICalculating;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (IsGameStarted && Turn == PieceColour.Black && !_isAICalculating) //check for AI turn
            {
                _isAICalculating = true;
                AINextMove();
                _isAICalculating = false;
            }
        }


        private void LogInfo(string log)
        {
            infoArea.Text = $"{log} \n" + infoArea.Text;
        }
    }
}
