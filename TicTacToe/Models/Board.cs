using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.Models
{
    public class Board
    {
        [Key] public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Movement> Movements { get; set; }
        public List<PlayerBoard> PlayerBoards { get; set; }
        public int NumberOfColumn { get; set; }
        public int NumberOfRows { get; set; }
        [NotMapped] public List<List<Player?>> FieldsConfiguration { get; set; }
        [NotMapped] public List<int> FreeFields { get; set; }

        public bool PositionIsNotAvailable(int movementPosition)
        {
            var copiedMovementPosition = movementPosition;
            return FreeFields?.Any(position => position == copiedMovementPosition) is false;
        }

        public void InitializeBoardConfiguration()
        {
            var freeFields = new List<int>();
            var boardConfiguration = new List<List<Player?>>();
            var positionCount = 1;

            for (int indexRow = 0; indexRow < NumberOfRows; indexRow++)
            {
                boardConfiguration.Add(new List<Player?>());
                for (int indexColumn = 0; indexColumn < NumberOfColumn; indexColumn++)
                {
                    var movement = Movements?.FirstOrDefault(m => m.Position == positionCount);

                    if (movement is not null)
                    {
                        boardConfiguration[indexRow].Add(movement.WhoMade);
                    }
                    else
                    {
                        boardConfiguration[indexRow].Add(null);
                        freeFields.Add(positionCount);
                    }

                    positionCount++;
                }
            }

            FieldsConfiguration = boardConfiguration;
            FreeFields = freeFields;
        }
    }
}

