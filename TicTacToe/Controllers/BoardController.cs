using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Controllers.Models;
using TicTacToe.DataAccessLayer;
using TicTacToe.Models;
using TicTacToe.Models.UpdatedModels;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    [Route("board")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardDAL boardDAL;
        private readonly IGameService gameService;
        private readonly IMapper mapper;

        public BoardController(IBoardDAL boardDAL, IGameService gameService, 
                                IMapper mapper)
        {
            this.boardDAL = boardDAL;
            this.gameService = gameService;
            this.mapper = mapper;
        }

        [HttpGet("get-boards")]
        public async Task<IActionResult> GetAllBoards()
        {
            var boards = await boardDAL.GetBoards();
            var updatedBoards = mapper.Map<IEnumerable<Board>, IEnumerable<UpdatedBoard>>(boards);

            return Ok(updatedBoards);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecificBoard(int id)
        {
            var board = await boardDAL.GetBoardById(id);

            if (board is null)
            {
                return NotFound();
            }

            return Ok(board);
        }

        [HttpPost("create-new-board")]
        public async Task<IActionResult> CreateNewBoard([FromBody] CreateBoard createBoard)
        {

            var boardSize = createBoard.BoardSize;
            var firstPlayerId = createBoard.FirstPlayerId;
            var secondPlayerId = createBoard.SecondPlayerId;

            try
            {
                var board = await gameService.CreateNewBoard(boardSize, firstPlayerId, secondPlayerId);
                return Ok(board);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
