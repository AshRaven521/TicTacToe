using TicTacToe.Models;

namespace TicTacToe.BusinessLogic
{
    public interface IBoardJudge
    {
        bool DrawGame(List<List<Player?>> fields);
        Tuple<int, int> GetRowAndColGivenAPosition(in int position, Board board);
        bool WonDiagonally(Board gameConfiguredBoard, in int lastMovementPosition);
        bool WonHorizontally(Board gameConfiguredBoard, in int lastMovementPosition);
        bool WonReverseDiagonally(Board gameConfiguredBoard, in int lastMovementPosition);
        bool WonVertically(Board gameConfiguredBoard, in int lastMovementPosition);
    }
}