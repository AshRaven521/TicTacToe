namespace TicTacToe.Controllers.Models
{
    public class CreateBoard
    {
        public int FirstPlayerId { get; set; }
        public int SecondPlayerId { get; set; }
        public string BoardSize { get; set; } = "3x3";
    }
}
