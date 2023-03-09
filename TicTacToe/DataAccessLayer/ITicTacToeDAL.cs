using TicTacToe.Models;

namespace TicTacToe.DataAccessLayer
{
    public interface ITicTacToeDAL
    {
        Task<Board> CreateMovementAndRefreshBoard(Movement movement, Board board);
        Task<Board?> GetBoardByItsId(int boardId);
        Task<Game?> GetGameByItsBoard(Board board);
        Task<Player?> GetPlayerByItsId(int playerId);
        Task<Player> GetSomeComputerPlayer();
        Task<Game> RefreshGameState(Game game);
        Task SaveBoard(Board board);
    }
}