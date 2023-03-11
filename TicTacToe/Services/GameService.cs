using AutoMapper;
using TicTacToe.BusinessLogic;
using TicTacToe.DataAccessLayer;
using TicTacToe.Models;
using TicTacToe.Models.UpdatedModels;

namespace TicTacToe.Services
{
    public class GameService : IGameService
    {
        private readonly IMapper mapper;
        private readonly IBoardDealer boardDealer;
        private readonly IGameDAL gameDAL;
        private readonly IBoardDAL boardDAL;
        private readonly IPlayerDAL playerDAL;

        public GameService(IMapper mapper,
                           IBoardDealer boardDealer,
                           IGameDAL gameDAL,
                           IBoardDAL boardDAL,
                           IPlayerDAL playerDAL)
        {
            this.mapper = mapper;
            this.boardDealer = boardDealer;
            this.gameDAL = gameDAL;
            this.boardDAL = boardDAL;
            this.playerDAL = playerDAL;
        }

        public async Task<UpdatedBoard> CreateNewBoard(string boardSize, int firstPlayerId, int secondPlayerId)
        {
            if (boardDealer.NotValidOrUnsupportedBoardSize(boardSize))
            {
                throw new Exception($"Board {boardSize} is not supported. You can try 3x3");
            }

            var playerOne = await playerDAL.GetPlayerById(firstPlayerId);
            var playerTwo = await playerDAL.GetPlayerById(secondPlayerId);
            if (playerOne is null || playerTwo is null)
            {
                string p1 = string.Empty;
                string p2 = string.Empty;
                if (playerOne?.Name is null)
                {
                    p1 = "No name";
                }
                else
                {
                    p1 = playerOne.Name;
                }
                if (playerTwo?.Name is null)
                {
                    p2 = "No name";
                }
                else
                {
                    p2 = playerTwo.Name;
                }
                throw new Exception($"Both players are required. P1: {p1} | P2: {p2}");
            }

            var freshNewBoard = boardDealer.PrepareBoardWithRequestSetup(boardSize, playerOne, playerTwo);
            await boardDAL.SaveBoard(freshNewBoard);

            return mapper.Map<Board, UpdatedBoard>(freshNewBoard);
        }

        public async Task<UpdatedGame> ExecuteMovement(int boardId, int playerId, int movementPosition)
        {
            var board = await boardDAL.GetBoardById(boardId);
            if (board is null)
            {
                throw new Exception($"The board {boardId} is not available. Are you sure you are correct? ");
            }

            var player = await playerDAL.GetPlayerById(playerId);
            if (player is null)
            {
                throw new Exception($"There is no player with ID {playerId}");
            }

            var game = await gameDAL.GetGameByBoard(board);
            if (game is null)
            {
                game = new Game(board);
            }
            if (game.Finished)
            {
                throw new Exception($"The game associated with the board {board.Id} is finished");
            }

            if (board.PositionIsNotAvailable(movementPosition))
            {
                List<int> freePositions = board.FreeFields;
                var positions = String.Join(" ", freePositions);
                throw new Exception($"Position {movementPosition} is not available. The ones you can choose: {positions}");
            }

            var result = await ExecuteMovementAndRetrieveResult(movementPosition, player, board);

            game.Draw = result.IsDraw;
            game.Finished = result.IsGameOver;
            if (result.IsGameOver && result.HasAWinner)
            {
                game.Winner = result.PlayerWhoMadeLastMove;
            }
            else
            {
                game.Winner = null;
            }
            await gameDAL.RefreshGameState(game);

            return mapper.Map<Game, UpdatedGame>(game);
        }

        private async Task<MovementResult> ExecuteMovementAndRetrieveResult(int movementPosition, Player player, Board board)
        {

            var createdMovement = boardDealer.CreateMovementForPlayer(board, movementPosition, player);
            await boardDAL.UpdateBoardWithMovement(createdMovement, board);
            var currentBoardState = boardDealer.EvaluateTheSituation(board, movementPosition);
            var noMoreMovementsAvailable = board.FreeFields.Count <= 0;
            var isGameOver = currentBoardState.HasAWinner || currentBoardState.IsDraw || noMoreMovementsAvailable;

            return new MovementResult(currentBoardState.HasAWinner, currentBoardState.IsDraw, noMoreMovementsAvailable, isGameOver, player);
        }

    }
}
