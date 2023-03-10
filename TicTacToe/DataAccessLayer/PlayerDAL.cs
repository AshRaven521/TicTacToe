using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;
using TicTacToe.Models;

namespace TicTacToe.DataAccessLayer
{
    public class PlayerDAL : IPlayerDAL
    {
        private readonly ApplicationDbContext context;

        public PlayerDAL(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Player?>> GetPlayers()
        {
            return await context.Players.ToListAsync();
        }
        public async Task<Player?> GetPlayerById(int playerId)
        {
            return await context.Players.FindAsync(playerId);
        }
        public async Task<Player?> CreatePlayer(Player player)
        {
            var newPlayer = await context.Players.AddAsync(player);
            await context.SaveChangesAsync();
            return newPlayer.Entity;
        }

        public async Task DeletePlayer(Player player)
        {
            context.Players.Remove(player);
            await context.SaveChangesAsync();
        }
    }
}
