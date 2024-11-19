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

        // Test with: curl -X POST "http://localhost:5256/game-state?playerSumValue=21&playerValueType=Blackjack&playerStateType=Terminal&dealerFaceUpValue=10&dealerValueType=Hard&dealerStateType=Active"
        [HttpPost("/game-state")]
        public IActionResult GenerateGameState(
            [FromQuery] int playerSumValue,
            [FromQuery] GameState.GameStateValueType playerValueType,
            [FromQuery] GameState.GameStateType playerStateType,
            [FromQuery] int dealerFaceUpValue,
            [FromQuery] GameState.GameStateValueType dealerValueType,
            [FromQuery] GameState.GameStateType dealerStateType
        )
        {
            var gameState = new GameState
            {
                PlayerSumValue = playerSumValue,
                PlayerValueType = playerValueType,
                PlayerStateType = playerStateType,
                DealerFaceUpValue = dealerFaceUpValue,
                DealerValueType = dealerValueType,
                DealerStateType = dealerStateType
            };

            _gameStateCollection.InsertOne(gameState);

            return Ok(gameState);
        }

        // Test with: curl -X GET "http://localhost:5256/game-state?playerSumValue=21&playerValueType=Blackjack&playerStateType=Terminal&dealerFaceUpValue=10&dealerValueType=Hard&dealerStateType=Active"
        [HttpGet("/game-state")]
        public IActionResult GetGameState(
            [FromQuery] int playerSumValue,
            [FromQuery] GameState.GameStateValueType playerValueType,
            [FromQuery] GameState.GameStateType playerStateType,
            [FromQuery] int dealerFaceUpValue,
            [FromQuery] GameState.GameStateValueType dealerValueType,
            [FromQuery] GameState.GameStateType dealerStateType
        )
        {
            var filter = Builders<GameState>.Filter.And(
                Builders<GameState>.Filter.Eq(g => g.PlayerSumValue, playerSumValue),
                Builders<GameState>.Filter.Eq(g => g.PlayerValueType, playerValueType),
                Builders<GameState>.Filter.Eq(g => g.PlayerStateType, playerStateType),
                Builders<GameState>.Filter.Eq(g => g.DealerFaceUpValue, dealerFaceUpValue),
                Builders<GameState>.Filter.Eq(g => g.DealerValueType, dealerValueType),
                Builders<GameState>.Filter.Eq(g => g.DealerStateType, dealerStateType)
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
