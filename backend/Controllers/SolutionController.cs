using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Solvation.Models;
using System.Linq;

namespace Solvation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolutionController : ControllerBase
    {
        private readonly IMongoCollection<GameState> _gameStateCollection;

        public SolutionController(MongoDbService mongoDbService)
        {
            _gameStateCollection = mongoDbService.GetCollection<GameState>("gameStates");
        }

        [HttpGet("/test")]
        public IActionResult GetTestResponse()
        {
            return Ok(new { message = "Test route" });
        }

        [HttpPost("/game-state")]
        public IActionResult GenerateGameState()
        {
            var gameState = new GameState
            {
                PlayerSumValue = 21,
                PlayerValueType = GameState.GameStateValueType.Blackjack,
                PlayerStateType = GameState.GameStateType.Terminal,
                DealerFaceUpValue = 10,
                DealerValueType = GameState.GameStateValueType.Hard,
                DealerStateType = GameState.GameStateType.Active
            };

            _gameStateCollection.InsertOne(gameState);

            return Ok(gameState);
        }

        [HttpGet("/game-state")]
        public IActionResult GetGameState(
            [FromQuery] int playerSumValue,
            [FromQuery] string playerValueType,
            [FromQuery] string playerStateType,
            [FromQuery] int dealerFaceUpValue,
            [FromQuery] string dealerValueType,
            [FromQuery] string dealerStateType
        )
        {
            if (!Enum.TryParse(playerValueType, true, out GameState.GameStateValueType parsedPlayerValueType) ||
                !Enum.TryParse(playerStateType, true, out GameState.GameStateType parsedPlayerStateType) ||
                !Enum.TryParse(dealerValueType, true, out GameState.GameStateValueType parsedDealerValueType) ||
                !Enum.TryParse(dealerStateType, true, out GameState.GameStateType parsedDealerStateType))
            {
                return BadRequest("Invalid enum values provided.");
            }

            var filter = Builders<GameState>.Filter.And(
                Builders<GameState>.Filter.Eq(g => g.PlayerSumValue, playerSumValue),
                Builders<GameState>.Filter.Eq(g => g.PlayerValueType, parsedPlayerValueType),
                Builders<GameState>.Filter.Eq(g => g.PlayerStateType, parsedPlayerStateType),
                Builders<GameState>.Filter.Eq(g => g.DealerFaceUpValue, dealerFaceUpValue),
                Builders<GameState>.Filter.Eq(g => g.DealerValueType, parsedDealerValueType),
                Builders<GameState>.Filter.Eq(g => g.DealerStateType, parsedDealerStateType)
            );

            var gameStates = _gameStateCollection.Find(filter).ToList();

            if (!gameStates.Any())
            {
                return NotFound("No matching GameStates found.");
            }

            if (gameStates.Count > 1)
            {
                return StatusCode(500, "Multiple GameStates found.");
            }

            return Ok(gameStates.First());
        }
    }
}
