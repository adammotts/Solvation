using Microsoft.AspNetCore.Mvc;
using Solvation.Services;

namespace Solvation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HandController : ControllerBase
    {
        private readonly IHandService _handService;
        private readonly IGameService _gameService;

        public HandController(IHandService handService, IGameService gameService)
        {
            _handService = handService;
            _gameService = gameService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHandById(string id)
        {
            try
            {
                var hand = await _handService.GetHandWithGameStateAsync(id);
                var gameState = await _gameService.GetActionsFromStatesAsync(
                    hand.CurrentPlayerState(), 
                    hand.CurrentDealerState()
                );

                return Ok(new { hand, actions = gameState });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(new { message = ex.Message });
                }
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetHands()
        {
            try
            {
                var hands = await _handService.GetHandsAsync();
                return Ok(string.Join("\n", hands.Select(element => element.ToString())));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // [HttpDelete]
        // public async Task<IActionResult> DeleteHands()
        // {
        //     try
        //     {
        //         await _handService.DeleteHandsAsync();
        //         return Ok();
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new { message = ex.Message });
        //     }
        // }
    }
}
