using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Solvation.Models;
using Solvation.Enums;
using Solvation.Requests;
using Solvation.Algorithms;

namespace Solvation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolutionController : ControllerBase
    {
        private readonly IMongoCollection<GameState> _gameStateCollection;

        private readonly IMongoCollection<Hand> _handCollection;

        private readonly IMongoCollection<Session> _sessionCollection;

        public SolutionController(MongoDbService mongoDbService)
        {
            _gameStateCollection = mongoDbService.GetCollection<GameState>("gameStates");
            _handCollection = mongoDbService.GetCollection<Hand>("hands");
            _sessionCollection = mongoDbService.GetCollection<Session>("sessions");
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
            curl -X PUT "http://localhost:5256/game-states"
        */
        [HttpPut("/game-states")]
        public IActionResult ResetGameStates()
        {
            _gameStateCollection.DeleteMany(Builders<GameState>.Filter.Empty);

            GameState[] gameStates = Solver.Solve();

            _gameStateCollection.InsertMany(gameStates);

            return Ok(string.Join("\n", gameStates.Select(element => element.ToString())));
        }

        /* Test with:
            curl -X GET "http://localhost:5256/moves" -H "Content-Type: application/json" -d '{ "PlayerCards": ["2♥", "10♥"], "DealerCard": "6♥" }'
        */
        [HttpGet("/moves")]
        public IActionResult GetMoves([FromBody] GetMovesRequest request)
        {
            Card[] playerCards = Card.FromStringArray(request.PlayerCards);
            Card dealerCard = Card.FromString(request.DealerCard);

            PlayerState playerState = PlayerState.FromCards(playerCards);
            DealerState dealerState = DealerState.FromCard(dealerCard);
            
            Actions actions = ActionsFromStates(playerState, dealerState);
                
            return Ok(actions);
        }

        /* Test with:
            curl -X POST "http://localhost:5256/session"  
        */
        [HttpPost("/session")]
        public IActionResult GenerateSession()
        {
            Hand[] hands = new Hand[10];

            for (int i = 0; i < hands.Length; i++)
            {
                hands[i] = new Hand();
            }

            _handCollection.InsertMany(hands);

            Session session = new Session(hands);

            _sessionCollection.InsertOne(session);

            return Ok(new { id = session.Id });
        }

        /* Test with:
            curl -X GET "http://localhost:5256/session/6765bf2e7f65239364166057"
        */
        [HttpGet("/session/{id}")]
        public IActionResult GetSessionById(string id)
        {
            Session session = SessionFromId(id);

            if (session.Ended())
            {
                return Ok(new { ended = true, evLoss = session.ExpectedValueLoss });
            }
            
            Hand hand = HandFromSession(session);

            PlayerState playerState = hand.CurrentPlayerState();
            DealerState dealerState = hand.CurrentDealerState();

            Actions actions = ActionsFromStates(playerState, dealerState);
            
            HandleTerminalPlayerState(session, playerState);

            return Ok(new { ended = false, hand, actions, terminal = playerState.StateType == GameStateType.Terminal });
        }

        [HttpPatch("/session/{id}")]
        public IActionResult MakeMoveForSession(string id, [FromBody] MakeMoveRequest request)
        {
            var session = SessionFromId(id);
            
            Hand hand = HandFromSession(session);

            var move = request.Move;

            UpdateSessionEV(session, hand, move);
            
            PlayerState playerState;

            switch (move)
            {
                case "hit":
                    playerState = hand.Hit();
                    break;
                case "stand":
                    playerState = hand.Stand();
                    break;
                case "double":
                    playerState = hand.Double();
                    break;
                case "split":
                    playerState = hand.Split();
                    break;
                default:
                    return BadRequest(new { message = "Invalid move" });
            }

            var handUpdate = Builders<Hand>.Update.Set(h => h.PlayerCards, hand.PlayerCards);
            _handCollection.UpdateOne(h => h.Id == hand.Id, handUpdate);

            HandleTerminalPlayerState(session, playerState);

            DealerState dealerState = hand.CurrentDealerState();

            Actions actions;
            
            try
            {
                actions = ActionsFromStates(playerState, dealerState);
            }
            catch
            {
                actions = new Actions(null, null, null, null);
            }

            return Ok(new { ended = false, hand, actions, terminal = playerState.StateType == GameStateType.Terminal });
        }

        /* Test with:
            curl -X DELETE "http://localhost:5256/sessions"
        */
        [HttpDelete("/sessions")]
        public IActionResult DeleteSessions()
        {
            _sessionCollection.DeleteMany(Builders<Session>.Filter.Empty);

            return Ok();
        }

        /* Test with:
            curl -X GET "http://localhost:5256/hand/6765085c864009ec961ea2e8"
        */
        [HttpGet("/hand/{id}")]
        public IActionResult GetHandById(string id)
        {
            var hand = _handCollection.Find(h => h.Id == id).FirstOrDefault();

            if (hand == null)
            {
                return NotFound(new { message = "Hand not found" });
            }

            Card[] playerCards = hand.PlayerCards.ToArray();
            Card dealerCard = hand.DealerCards.First();

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
                return StatusCode(500, "Multiple GameStates found. " + gameStates.Count);
            }

            return Ok(new { hand, gameStates.First().Actions });
        }

        /* Test with:
            curl -X GET "http://localhost:5256/hands"
        */
        [HttpGet("/hands")]
        public IActionResult GetHands()
        {
            return Ok(string.Join("\n", _handCollection.Find(Builders<Hand>.Filter.Empty).ToList().Select(element => element.ToString())));
        }

        /* Test with:
            curl -X DELETE "http://localhost:5256/hands"
        */
        [HttpDelete("/hands")]
        public IActionResult DeleteHands()
        {
            _handCollection.DeleteMany(Builders<Hand>.Filter.Empty);

            return Ok();
        }

        private Actions ActionsFromStates(PlayerState playerState, DealerState dealerState)
        {
            var filter = Builders<GameState>.Filter.And(
                Builders<GameState>.Filter.Eq(g => g.PlayerState, playerState),
                Builders<GameState>.Filter.Eq(g => g.DealerState, dealerState)
            );

            var gameStates = _gameStateCollection.Find(filter).ToList();

            if (!gameStates.Any())
            {
                throw new System.Exception("No matching GameStates found.");
            }

            if (gameStates.Count > 1)
            {
                throw new System.Exception("Multiple GameStates found. " + gameStates.Count);
            }

            return gameStates.First().Actions;
        }

        private Session SessionFromId(string id)
        {
            var session = _sessionCollection.Find(s => s.Id == id).FirstOrDefault();

            if (session == null)
            {
                throw new System.Exception("Session not found");
            }

            return session;
        }

        private Hand HandFromSession(Session session)
        {
            var currentHandId = session.CurrentHandId();

            var hand = _handCollection.Find(h => h.Id == currentHandId).FirstOrDefault();

            if (hand == null)
            {
                throw new System.Exception("Hand not found");
            }

            return hand;
        }

        private void HandleTerminalPlayerState(Session session, PlayerState playerState)
        {
            if (playerState.StateType == GameStateType.Terminal)
            {
                session.NextHand();
            }

            var sessionUpdate = Builders<Session>.Update.Set(s => s.CurrentHandIndex, session.CurrentHandIndex);
            _sessionCollection.UpdateOne(s => s.Id == session.Id, sessionUpdate);
        }

        private void UpdateSessionEV(Session session, Hand hand, string move)
        {
            Actions actions = ActionsFromStates(hand.CurrentPlayerState(), hand.CurrentDealerState());

            double evBestMove = actions.BestMoveEV();
            double evMove = actions.MoveEV(move);

            var sessionUpdate = Builders<Session>.Update
                .Set(s => s.ExpectedValueLoss, session.ExpectedValueLoss + evBestMove - evMove);
            _sessionCollection.UpdateOne(s => s.Id == session.Id, sessionUpdate);
        }
    }
}
