using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models
{
    public class Player
    {
        [Key] public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<PlayerBoard>? PlayerBoards { get; set; }
        public string Name { get; set; }
    }
}
