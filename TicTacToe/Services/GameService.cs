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
        private readonly IBoardDealer _boardDealer;
        private readonly ITicTacToeDAL repository;

        public GameService(IMapper mapper, IBoardDealer boardDealer,
                           ITicTacToeDAL repository)
        {
            this.mapper = mapper;
            _boardDealer = boardDealer;
            this.repository = repository;
        }

        public async Task<UpdatedBoard> CreateNewBoard(string boardSize, int firstPlayerId, int secondPlayerId)
        {
            //Log.Information("Checking board setup");
            if (_boardDealer.NotValidOrUnsupportedBoardSize(boardSize))
            {
                string message = $"Board {boardSize} is not supported. You can try 3x3 👍";
                throw new Exception(message);
            }

            //Log.Information("Checking players");
            var playerOne = await repository.GetPlayerByItsId(firstPlayerId);
            var playerTwo = await repository.GetPlayerByItsId(secondPlayerId);
            if (playerOne is null || playerTwo is null)
            {
                var p1 = playerOne?.Name is null ? "❓" : playerOne.Name;
                var p2 = playerTwo?.Name is null ? "❓" : playerTwo.Name;
                string message = $"Both players are required. P1: {p1} | P2: {p2}";
                throw new Exception(message);
            }

            //Log.Information("Creating board");
            var freshNewBoard = _boardDealer.PrepareBoardWithRequestSetup(boardSize, playerOne, playerTwo);
            await repository.SaveBoard(freshNewBoard);

            return mapper.Map<Board, UpdatedBoard>(freshNewBoard);
        }

        public async Task<UpdatedGame> ExecuteMovementAndRetrieveGameStatus(int boardId, int playerId, int movementPosition)
        {
            //Log.Information("Searching the board 🎮");
            var board = await repository.GetBoardByItsId(boardId);
            if (board is null)
            {
                string message = $"The board {boardId} is not available. Are you sure you are correct? 🤔";
                throw new Exception(message);
            }

            //Log.Information("Searching the user 🕹");
            var player = await repository.GetPlayerByItsId(playerId);
            if (player is null)
            {
                throw new Exception($"There is no player with ID {playerId}");
            }
            if (player.Computer)
            {
                throw new Exception($"{player.Name} is a robot. Only I can use it!");
            }

            //Log.Information("Searching for a game 🎰");
            var game = await repository.GetGameByItsBoard(board);
            if (game is null)
            {
                game = new Game(board);
            }
            if (game.IsFinished())
            {
                throw new Exception($"The game associated with the board {board.Id} is finished");
            }

            //Log.Information("Checking position 🕵");
            if (board.PositionIsNotAvailable(movementPosition))
            {
                IList<int> freePositions = board.FreeFields;
                var positions = String.Join(" ", freePositions);
                var message = $"Position {movementPosition} is not available. The ones you can choose: {positions}";
                throw new Exception(message);
            }

            var result = await ExecuteMovementAndRetrieveResult(movementPosition, player, board);
            if (result.IsGameOver is false)
            {
                var robotPlayer = board.GetRobotPlayer();
                //if (robotPlayer is not null)
                //{
                //    int position = _positionDecider.ChooseTheBestAvailablePositionFor(board.FreeFields);
                //    result = await ExecuteMovementAndRetrieveResult(position, robotPlayer, board);
                //}
            }

            //Log.Information("Updating game state 📝");
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
            //game.Winner = result.IsGameOver && result.HasAWinner ? result.PlayerWhoMadeLastMove : null;
            await repository.RefreshGameState(game);

            return mapper.Map<Game, UpdatedGame>(game);
        }

        private async Task<MovementResult> ExecuteMovementAndRetrieveResult(int movementPosition, Player player, Board board)
        {
            //Log.Information($"Executing movement for player {player.Name} and evaluating game 🔍");

            var createdMovement = _boardDealer.CreateMovementForCustomPlayerOrComputer(board, movementPosition, player);
            await repository.CreateMovementAndRefreshBoard(createdMovement, board);
            var currentBoardState = _boardDealer.EvaluateTheSituation(board, movementPosition);
            var noMoreMovementsAvailable = board.FreeFields.Count <= 0;
            var isGameOver = currentBoardState.HasAWinner || currentBoardState.IsDraw || noMoreMovementsAvailable;

            return new MovementResult(currentBoardState.HasAWinner, currentBoardState.IsDraw, noMoreMovementsAvailable, isGameOver, player);
        }

        //public record MovementResult(bool HasAWinner, bool IsDraw, bool NoMoreMovementsAvailable, bool IsGameOver, Player WhoDidTheLastMovement);
    }
}
