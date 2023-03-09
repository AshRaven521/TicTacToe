namespace TicTacToe.Models.UpdatedModels
{
    public class UpdatedGame
    {
        public long Id { get; set; }
        public UpdatedPlayer Winner { get; set; }
        public bool Draw { get; set; }
        public bool Finished { get; set; }
        public UpdatedBoard ConfiguredBoard { get; set; }
    }
}
