using AutoMapper;
using DrfLikePaginations;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;
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
        private readonly ITicTacToeDAL ticTacToeDAL;

        public PlayerController(IMapper mapper, ITicTacToeDAL ticTacToeDAL)
        {
            this.mapper = mapper;
            this.ticTacToeDAL = ticTacToeDAL;
        }

        [HttpGet("get-players")]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await ticTacToeDAL.GetPlayers();
            var newPlayers = mapper.Map<IEnumerable<Player>, IEnumerable<UpdatedPlayer>>(players);

            return Ok(newPlayers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecificPlayer(int id)
        {
            var player = await ticTacToeDAL.GetPlayerById(id);

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

            var newPlayer = await ticTacToeDAL.CreatePlayer(player);
            return Ok(newPlayer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await ticTacToeDAL.GetPlayerById(id);

            if (player is null)
            {
                return NotFound();
            }

            await ticTacToeDAL.DeletePlayer(player);

            return Ok();
        }
    }
}
