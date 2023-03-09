namespace TicTacToe.Models
{
    public class MovementResult
    {
        public bool HasAWinner { get; set; }
        public bool IsDraw { get; set; }
        public bool NoMovementsAvailable { get; set; }
        public bool IsGameOver { get; set; }
        public Player PlayerWhoMadeLastMove { get; set; }

        public MovementResult(bool winner, bool draw, bool noMovements, bool gameOver, Player lastMovedPlayer)
        {
            HasAWinner = winner;
            IsDraw = draw;
            NoMovementsAvailable = noMovements;
            IsGameOver = gameOver;
            PlayerWhoMadeLastMove = lastMovedPlayer;
        }
    }
}
