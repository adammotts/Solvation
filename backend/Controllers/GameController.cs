using Microsoft.AspNetCore.Mvc;
using Solvation.Services;

namespace Solvation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("expected-value")]
        public async Task<IActionResult> GenerateGameExpectedValue()
        {
            try
            {
                double gameExpectedValue = await _gameService.GenerateGameExpectedValueAsync();
                return Ok(gameExpectedValue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // [HttpDelete("expected-values")]
        // public async Task<IActionResult> DeleteGameExpectedValues()
        // {
        //     try
        //     {
        //         await _gameService.DeleteGameExpectedValuesAsync();
        //         return Ok();
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new { message = ex.Message });
        //     }
        // }

        [HttpGet("expected-value")]
        public async Task<IActionResult> GetGameExpectedValue()
        {
            try
            {
                var gameExpectedValue = await _gameService.GetGameExpectedValueAsync();
                return Ok(gameExpectedValue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("states")]
        public async Task<IActionResult> GenerateGameStates()
        {
            try
            {
                var gameStates = await _gameService.GenerateGameStatesAsync();
                return Ok(string.Join("\n", gameStates.Select(element => element.ToString())));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // [HttpDelete("states")]
        // public async Task<IActionResult> DeleteGameStates()
        // {
        //     try
        //     {
        //         await _gameService.DeleteGameStatesAsync();
        //         return Ok();
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new { message = ex.Message });
        //     }
        // }

        [HttpGet("states")]
        public async Task<IActionResult> GetGameStates()
        {
            try
            {
                var gameStates = await _gameService.GetGameStatesAsync();
                return Ok(string.Join("\n", gameStates.Select(element => element.ToString())));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("states")]
        public async Task<IActionResult> ResetGameStates()
        {
            try
            {
                var gameStates = await _gameService.ResetGameStatesAsync();
                return Ok(string.Join("\n", gameStates.Select(element => element.ToString())));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
