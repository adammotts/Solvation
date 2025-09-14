using Microsoft.AspNetCore.Mvc;
using Solvation.Services;
using Solvation.Models;
using Solvation.Requests;

namespace Solvation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public MovesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMoves([FromBody] GetMovesRequest request)
        {
            try
            {
                Card[] playerCards = Card.FromStringArray(request.PlayerCards);
                Card dealerCard = Card.FromString(request.DealerCard);

                PlayerState playerState = PlayerState.FromCards(playerCards);
                DealerState dealerState = DealerState.FromCard(dealerCard);

                Actions actions = await _gameService.GetActionsFromStatesAsync(playerState, dealerState);

                return Ok(actions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
