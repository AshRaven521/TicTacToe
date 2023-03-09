using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models
{
    public class PlayerBoard
    {
        [Key] public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }
    }
}
