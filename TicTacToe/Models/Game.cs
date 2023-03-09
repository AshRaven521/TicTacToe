using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models
{
    public class Game
    {
        [Key] public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Player? Winner { get; set; }
        public bool Draw { get; set; }
        public bool Finished { get; set; }
        public Board ConfiguredBoard { get; set; }

        public Game()
        {
            
        }

        public Game(Board board)
        {
            Finished = false;
            board.InitializeBoardConfiguration();
            ConfiguredBoard = board;
        }

        public bool IsFinished()
        {
            return Finished;
        }
    }
}
