using TicTacToe.Models;

namespace TicTacToe.DataAccessLayer
{
    public interface IGameDAL
    {
        Task<Game?> GetGameByBoard(Board board);
        Task<Game> GetGameById(int id);
        Task<IEnumerable<Game?>> GetGames();
        Task<Game> RefreshGameState(Game game);
    }
}