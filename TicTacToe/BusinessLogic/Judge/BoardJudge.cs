using TicTacToe.Models;

namespace TicTacToe.BusinessLogic
{
    public class BoardJudge : IBoardJudge
    {
        public Tuple<int, int> GetRowAndColGivenAPosition(in int position, Board board)
        {
            var refreshedMovementPosition = position - 1;

            var row = refreshedMovementPosition / board.NumberOfRows;
            var col = refreshedMovementPosition % board.NumberOfColumn;

            return Tuple.Create(row, col);
        }

        public bool WonHorizontally(Board gameConfiguredBoard, in int lastMovementPosition)
        {
            var (row, _) = GetRowAndColGivenAPosition(lastMovementPosition, gameConfiguredBoard);

            var fields = gameConfiguredBoard.FieldsConfiguration;
            var playerUsedToEvaluate = fields[row].First();
            bool isPlayerPresentInAllHorizontalFields = fields[row].All(p => p is not null && p.Equals(playerUsedToEvaluate));

            return isPlayerPresentInAllHorizontalFields;
        }

        public bool WonVertically(Board gameConfiguredBoard, in int lastMovementPosition)
        {
            var (row, col) = GetRowAndColGivenAPosition(lastMovementPosition, gameConfiguredBoard);

            var fields = gameConfiguredBoard.FieldsConfiguration;
            var playerUsedToEvaluate = fields[row][col];
            var isPlayerPresentInAllVerticalFields = fields.All(row => 
                                                          row[col] is not null && row[col].Equals(playerUsedToEvaluate));

            return isPlayerPresentInAllVerticalFields;
        }

        public bool WonDiagonally(Board gameConfiguredBoard, in int lastMovementPosition)
        {
            var (row, col) = GetRowAndColGivenAPosition(lastMovementPosition, gameConfiguredBoard);

            if (row != col)
            {
                return false;
            }

            var fields = gameConfiguredBoard.FieldsConfiguration;
            var foundPlayer = fields[row][col];
            var columnCounter = 0;
            Func<List<Player?>, bool> predicate = row =>
            {
                var maybeAPlayerHere = row[columnCounter++];
                return maybeAPlayerHere is not null && maybeAPlayerHere.Equals(foundPlayer);
            };

            return fields.All(predicate);
        }

        public bool WonReverseDiagonally(Board gameConfiguredBoard, in int lastMovementPosition)
        {
            var (row, col) = GetRowAndColGivenAPosition(lastMovementPosition, gameConfiguredBoard);

            var isNotEligible = (row + col) != (gameConfiguredBoard.NumberOfRows - 1);

            if (isNotEligible)
            {
                return false;
            }

            var fields = gameConfiguredBoard.FieldsConfiguration;
            var foundPlayer = fields[row][col];
            var columnCounter = fields[row].Count - 1;

            bool Predicate(List<Player?> row)
            {
                var maybeAPlayerHere = row[columnCounter--];
                return maybeAPlayerHere is not null && maybeAPlayerHere.Equals(foundPlayer);
            }

            return fields.All(Predicate);
        }

        public bool DrawGame(List<List<Player?>> fields)
        {
            return fields.All(row => row.All(col => col is not null));
        }
    }
}
