using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;
using TicTacToe.Models;

namespace TicTacToe.DataAccessLayer
{
    public class BoardDAL : IBoardDAL
    {
        private readonly ApplicationDbContext context;

        public BoardDAL(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Board>> GetBoards()
        {
            return await context.Boards.ToListAsync();
        }

        public async Task<Board?> GetBoardById(int boardId)
        {
            var boards = await context.Boards
                .Where(b => b.Id == boardId)
                .Include(b => b.PlayerBoards)
                .ThenInclude(pb => pb.Player)
                .ToListAsync();

            return boards.FirstOrDefault();
        }


        public async Task SaveBoard(Board board)
        {
            foreach (var playerBoard in board.PlayerBoards)
            {
                var p = await context.Players.FindAsync(playerBoard?.Player?.Id);
                playerBoard.Player = p;
            }
            context.Boards.Add(board);
            await context.SaveChangesAsync();
        }

        public async Task<Board> UpdateBoardWithMovement(Movement movement, Board board)
        {
            if (board.Movements is null)
            {
                board.Movements = new List<Movement>();
            }

            board.Movements.Add(movement);

            var entityEntryBoard = context.Boards.Update(board);
            await context.SaveChangesAsync();

            return entityEntryBoard.Entity;
        }
    }
}
