using TicTacToe.Models;

namespace TicTacToe.BusinessLogic
{
    public interface IBoardDealer
    {
        Movement CreateMovementForPlayer(Board board, int position, Player? player = null);
        BoardState EvaluateTheSituation(Board board, in int lastMovementPosition);
        bool NotValidOrUnsupportedBoardSize(string boardSize);
        Board PrepareBoardWithRequestSetup(string boardSize, Player playerOne, Player playerTwo);
    }
}