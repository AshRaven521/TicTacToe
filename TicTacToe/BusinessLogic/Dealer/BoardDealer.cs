using System.Diagnostics;
using System.Text.RegularExpressions;
using TicTacToe.Models;

namespace TicTacToe.BusinessLogic
{
    public class BoardDealer : IBoardDealer
    {
        private IBoardJudge boardJudge;
        private Regex boardValidation = new(@"[3-9]x[3-9]");

        public BoardDealer(IBoardJudge boardJudge)
        {
            this.boardJudge = boardJudge;
        }

        public bool NotValidOrUnsupportedBoardSize(string boardSize)
        {
            if (!boardValidation.IsMatch(boardSize))
            {
                return true;
            }

            var column = boardSize.Substring(0, 1);
            var rows = boardSize.Substring(2, 1);

            return column != rows;
        }

        public Board PrepareBoardWithRequestSetup(string boardSize, Player playerOne, Player playerTwo)
        {
            var column = int.Parse(boardSize.Substring(0, 1));
            var rows = int.Parse(boardSize.Substring(2, 1));

            var board = new Board { NumberOfColumn = column, NumberOfRows = rows };
            var playerBoardOne = new PlayerBoard { Player = playerOne, Board = board };
            var playerBoarTwo = new PlayerBoard { Player = playerTwo, Board = board };
            board.PlayerBoards = new List<PlayerBoard> { playerBoardOne, playerBoarTwo };
            board.InitializeBoardConfiguration();

            return board;
        }

        public Movement CreateMovementForPlayer(Board board, int position, Player? player = null)
        {
            if (player is null)
            {
                player = board.PlayerBoards.First().Player;
            }

            var (row, col) = boardJudge.GetRowAndColGivenAPosition(position, board);

            board.FieldsConfiguration[row][col] = player;

            return new Movement { Position = position, WhoMade = player };
        }

        public BoardState EvaluateTheSituation(Board board, in int lastMovementPosition)
        {
            bool wonHorizontally = boardJudge.WonHorizontally(board, lastMovementPosition);
            bool wonVertically = boardJudge.WonVertically(board, lastMovementPosition);
            bool wonDiagonally = boardJudge.WonDiagonally(board, lastMovementPosition);
            bool wonReverseDiagonally = boardJudge.WonReverseDiagonally(board, lastMovementPosition);

            bool hasAWinner = wonHorizontally || wonVertically || wonDiagonally || wonReverseDiagonally;
            var fields = board.FieldsConfiguration;
            bool drawGame = boardJudge.DrawGame(fields);

            return new BoardState(hasAWinner, drawGame);
        }
    }
}
