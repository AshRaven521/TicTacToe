using TicTacToe.Models;
using TicTacToe.Models.UpdatedModels;

namespace TicTacToe.Mapping
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            CreateMap<PlayerBoard, UpdatedPlayerBoard>();
            CreateMap<Board, UpdatedBoard>().
                ForMember(member => member.Players,
                          memberOptions => memberOptions.MapFrom(
                              source => source.PlayerBoards.Select(p => p.Player)));
            CreateMap<Player, UpdatedPlayer>();
            CreateMap<Game, UpdatedGame>();
        }
    }
}
