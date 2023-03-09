using TicTacToe.Models.UpdatedModels;

namespace TicTacToe.Services
{
    public interface IGameService
    {
        Task<UpdatedBoard> CreateNewBoard(string boardSize, int firstPlayerId, int secondPlayerId);
        Task<UpdatedGame> ExecuteMovementAndRetrieveGameStatus(int boardId, int playerId, int movementPosition);
    }
}