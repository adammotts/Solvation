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

            return Ok(string.Join("\n", gameStates.Select(element => element.ToString())));
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

        /* Test with:
            curl -X GET "http://localhost:5256/game-states"
        */
        [HttpGet("/game-states")]
        public IActionResult GetGameStates()
        {
            return Ok(string.Join("\n", _gameStateCollection.Find(Builders<GameState>.Filter.Empty).ToList().Select(element => element.ToString())));
        }

        /* Test with:
            curl -X GET "http://localhost:5256/moves" \
            -H "Content-Type: application/json" \
            -d '{
                "PlayerCards": ["2♥", "10♥"],
                "DealerCard": "6♥"
            }'
        */
        [HttpGet("/moves")]
        public IActionResult GetMoves([FromBody] GetMovesRequest request)
        {
            Card[] playerCards = Card.FromStringArray(request.PlayerCards);
            Card dealerCard = Card.FromString(request.DealerCard);

            PlayerState playerState = PlayerState.FromCards(playerCards);
            DealerState dealerState = DealerState.FromCard(dealerCard);

            var filter = Builders<GameState>.Filter.And(
                Builders<GameState>.Filter.Eq(g => g.PlayerState, playerState),
                Builders<GameState>.Filter.Eq(g => g.DealerState, dealerState)
            );

            var gameStates = _gameStateCollection.Find(filter).ToList();

            if (!gameStates.Any())
            {
                return NotFound("No matching GameStates found.");
            }

            if (gameStates.Count > 1)
            {
                return StatusCode(500, "Multiple GameStates found." + gameStates.Count);
            }

            return Ok(gameStates.First().Actions.ToString());
        }
    }
}
