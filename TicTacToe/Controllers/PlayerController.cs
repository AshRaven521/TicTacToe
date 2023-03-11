using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.DataAccessLayer;
using TicTacToe.Models;
using TicTacToe.Models.UpdatedModels;

namespace TicTacToe.Controllers
{
    [Route("player")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPlayerDAL playerDAL;

        public PlayerController(IMapper mapper, IPlayerDAL playerDAL)
        {
            this.mapper = mapper;
            this.playerDAL = playerDAL;
        }

        [HttpGet("get-players")]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await playerDAL.GetPlayers();
            var updatedPlayers = mapper.Map<IEnumerable<Player>, IEnumerable<UpdatedPlayer>>(players);

            return Ok(updatedPlayers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecificPlayer(int id)
        {
            var player = await playerDAL.GetPlayerById(id);

            if (player is null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpPost("create-player")]
        public async Task<IActionResult> CreateNewPlayer(Player player)
        {
            if (player is null)
            {
                return BadRequest();
            }

            var newPlayer = await playerDAL.CreatePlayer(player);
            return Ok(newPlayer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await playerDAL.GetPlayerById(id);

            if (player is null)
            {
                return NotFound();
            }

            await playerDAL.DeletePlayer(player);

            return Ok();
        }
    }
}
