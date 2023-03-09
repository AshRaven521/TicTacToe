using Microsoft.EntityFrameworkCore;
using System;
using TicTacToe.Data;
using TicTacToe.Models;

namespace TicTacToe.DataAccessLayer
{
    public class TicTacToeDAL : ITicTacToeDAL
    {
        private ApplicationDbContext appDbContext;

        public TicTacToeDAL(ApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Player?> GetPlayerByItsId(int playerId)
        {
            return await appDbContext.Players.FindAsync(playerId);
        }

        public async Task<Board?> GetBoardByItsId(int boardId)
        {
            var boards = await appDbContext.Boards
                .Where(b => b.Id == boardId)
                .Include(b => b.PlayerBoards)
                .ThenInclude(pb => pb.Player)
                .ToListAsync();

            return boards.FirstOrDefault();
        }

        public async Task<Game?> GetGameByItsBoard(Board board)
        {
            var games = await appDbContext.Games
                .Where(game => game.ConfiguredBoard.Id == board.Id)
                .Include(g => g.ConfiguredBoard)
                .ThenInclude(b => b.Movements)
                .ToListAsync();
            var possibleGame = games.FirstOrDefault();
            // To refresh board status
            possibleGame?.ConfiguredBoard?.InitializeBoardConfiguration();
            return possibleGame;
        }

        public async Task<Game> RefreshGameState(Game game)
        {
            // https://docs.microsoft.com/en-us/ef/core/saving/basic#updating-data
            var entityEntry = appDbContext.Games.Update(game);
            // TODO: Maybe disable AutoDetectChangesEnabled as it is enabled by default;
            var stateEntriesWrittenToTheDatabase = await appDbContext.SaveChangesAsync();
            // TODO: Maybe log stateEntriesWrittenToTheDatabase?
            return entityEntry.Entity;
        }

        public async Task<Player> GetSomeComputerPlayer()
        {
            return await appDbContext.Players.FirstAsync(p => p.Computer);
        }

        public async Task SaveBoard(Board board)
        {
            foreach (var playerBoard in board.PlayerBoards)
            {
                // TODO: Proposed solution: https://stackoverflow.com/a/39165051/3899136
                var p = await appDbContext.Players.FindAsync(playerBoard.Player.Id);
                playerBoard.Player = p;
            }
            appDbContext.Boards.Add(board);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<Board> CreateMovementAndRefreshBoard(Movement movement, Board board)
        {
            if (board.Movements is null)
                board.Movements = new List<Movement>();

            board.Movements.Add(movement);

            var entityEntryBoard = appDbContext.Boards.Update(board);
            await appDbContext.SaveChangesAsync();

            return entityEntryBoard.Entity;
        }
    }
}
