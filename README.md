# ChessAI
ChessAI is a project written in C#, where a player plays against the computer

![alt text](https://maciti.github.io/assets/ChessAI/board.PNG)

The project is written using .NET Core 3.1

It's composed by 3 subprojects:

<h3>ChessUI</h3>

A simple windows form project

<h3>ChessLibrary</h3>

A class library project that contains the logic of the game. The main class that contains the logic is Board.cs

<h3>AIPlayerLibrary</h3>

A class library project that plays against the user. <br/>
The main class is AIPlayer.cs that is responsible to calculate the next move. <br/>
The algorithm used by AIPlayer.CalculateNextMove(Board) method is a minimax implementation with alpha-beta pruning

<h3>TODO (limitations)</h3>
<ul>
  <li>Castling not implemented</li>
  <li>Promotion not implemented</li>
  <li>Clocks</li>
 </ul>
  

