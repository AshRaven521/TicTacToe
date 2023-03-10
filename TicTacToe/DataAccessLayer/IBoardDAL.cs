using TicTacToe.Models;

namespace TicTacToe.DataAccessLayer
{
    public interface IBoardDAL
    {
        Task<Board?> GetBoardById(int boardId);
        Task<IEnumerable<Board>> GetBoards();
        Task SaveBoard(Board board);
        Task<Board> UpdateBoardWithMovement(Movement movement, Board board);
    }
}