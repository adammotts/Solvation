using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Solvation.Models;
using Solvation.Requests;
using Solvation.Algorithms;

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

        /*
        Test with:
            curl -X POST "http://localhost:5256/game-states"
        */
        [HttpPost("/game-states")]
        public IActionResult GenerateGameStates()
        {
            GameState[] gameStates = Solver.Solve();

            _gameStateCollection.InsertMany(gameStates);

            return Ok(gameStates);
        }

        /* Test with:
            curl -X GET "http://localhost:5256/game-state" \
            -H "Content-Type: application/json" \
            -d '{
                "PlayerSumValue": 21,
                "PlayerValueType": "Blackjack",
                "PlayerStateType": "Terminal",
                "DealerFaceUpValue": 10,
                "DealerValueType": "Hard",
                "DealerStateType": "Active"
            }'
        */
        [HttpGet("/game-state")]
        public IActionResult GetGameState([FromBody] GetGameStateRequest request)
        {
            var filter = Builders<GameState>.Filter.And(
                Builders<GameState>.Filter.Eq(g => g.PlayerSumValue, request.PlayerSumValue),
                Builders<GameState>.Filter.Eq(g => g.PlayerValueType, request.PlayerValueType),
                Builders<GameState>.Filter.Eq(g => g.PlayerStateType, request.PlayerStateType),
                Builders<GameState>.Filter.Eq(g => g.DealerFaceUpValue, request.DealerFaceUpValue),
                Builders<GameState>.Filter.Eq(g => g.DealerValueType, request.DealerValueType),
                Builders<GameState>.Filter.Eq(g => g.DealerStateType, request.DealerStateType)
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

        /* Test with:
            curl -X DELETE "http://localhost:5256/game-states"
        */
        [HttpDelete("/game-states")]
        public IActionResult DeleteGameStates()
        {
            _gameStateCollection.DeleteMany(Builders<GameState>.Filter.Empty);

            return Ok();
        }
    }
}
