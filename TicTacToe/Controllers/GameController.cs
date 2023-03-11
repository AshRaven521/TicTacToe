using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Controllers.Models;
using TicTacToe.DataAccessLayer;
using TicTacToe.Models;
using TicTacToe.Models.UpdatedModels;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    [Route("game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameDAL gameDAL;
        private readonly IGameService gameService;
        private readonly IMapper mapper;

        public GameController(IGameDAL gameDAL, 
                                IGameService gameService, 
                                IMapper mapper)
        {
            this.gameDAL = gameDAL;
            this.gameService = gameService;
            this.mapper = mapper;
        }

        [HttpGet("get-games")]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await gameDAL.GetGames();
            var updatedGames = mapper.Map<IEnumerable<Game>, IEnumerable<UpdatedGame>>(games);

            return Ok(updatedGames);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCurrentGameStatus(int id)
        {
            var game = await gameDAL.GetGameById(id);

            if (game is null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        [HttpGet("play-game")]
        public async Task<IActionResult> ApplyMovementToTheGame([FromQuery] PlayGame playGame)
        {
            var boardId = playGame.BoardId;
            var movementPosition = playGame.MovementPosition;
            var playerId = playGame.PlayerId;

            try
            {
                var game = await gameService.ExecuteMovement(boardId, playerId, movementPosition);
                return Ok(game);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
