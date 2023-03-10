using TicTacToe.Models;

namespace TicTacToe.DataAccessLayer
{
    public interface IPlayerDAL
    {
        Task<Player?> CreatePlayer(Player player);
        Task DeletePlayer(Player player);
        Task<Player?> GetPlayerById(int playerId);
        Task<IEnumerable<Player?>> GetPlayers();
    }
}