using TicTacToe.Models;

namespace TicTacToe.BusinessLogic
{
    public class BoardSituation
    {
        public bool HasAWinner { get; set; }
        public bool IsDraw { get; set; }
        public Player Winner { get;set; }
        public bool Concluded { get; set; }
        
    }
}
