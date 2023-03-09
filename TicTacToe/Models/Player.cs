using System.ComponentModel.DataAnnotations;

namespace TicTacToe.Models
{
    public class Player
    {
        [Key] public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<PlayerBoard> PlayerBoards { get; set; }
        public string Name { get; set; }
        public bool Computer { get; set; }

        public bool isNotComputer()
        {
            return !Computer;
        }
    }
}
