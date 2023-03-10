using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;
using TicTacToe.Models;

namespace TicTacToe.DataAccessLayer
{
    public class GameDAL : IGameDAL
    {
        private readonly ApplicationDbContext context;

        public GameDAL(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Game?>> GetGames()
        {
            return await context.Games.ToListAsync();
        }

        public async Task<Game?> GetGameByBoard(Board board)
        {
            var games = await context.Games
                .Where(game => game.ConfiguredBoard.Id == board.Id)
                .Include(g => g.ConfiguredBoard)
                .ThenInclude(b => b.Movements)
                .ToListAsync();
            var possibleGame = games.FirstOrDefault();
            // To refresh board status
            possibleGame?.ConfiguredBoard?.InitializeBoardConfiguration();
            return possibleGame;
        }
        public async Task<Game> GetGameById(int id)
        {
            return await context.Games.FindAsync(id);
        }
        public async Task<Game> RefreshGameState(Game game)
        {
            var entityEntry = context.Games.Update(game);
            await context.SaveChangesAsync();
            return entityEntry.Entity;
        }
    }
}
