namespace TicTacToe.Models.UpdatedModels
{
    public class UpdatedBoard
    {
        public long Id { get; set; }
        public List<UpdatedPlayer> Players { get; set; }
        public int NumberOfColumn { get; set; }
        public int NumberOfRows { get; set; }
        public List<List<UpdatedPlayer?>>? FieldsConfiguration { get; set; }
        public List<int>? FreeFields { get; set; }
    }
}
