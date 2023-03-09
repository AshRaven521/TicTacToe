namespace TicTacToe.BusinessLogic
{
    public class BoardState
    {
        public bool HasAWinner { get; set; }
        public bool IsDraw { get; set; }

        public BoardState(bool winner, bool draw)
        {
            HasAWinner = winner;
            IsDraw = draw;
        }
    }
}
