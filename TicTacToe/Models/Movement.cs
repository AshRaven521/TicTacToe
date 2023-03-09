using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models
{
    public class Movement
    {
        [Key] public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Position { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public Player WhoMade { get; set; }
    }
}
