using Microsoft.AspNetCore.Mvc;
using Solvation.Services;
using Solvation.Models;
using Solvation.Requests;
using Solvation.Enums;

namespace Solvation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly IHandService _handService;
        private readonly IGameService _gameService;

        public SessionController(ISessionService sessionService, IHandService handService, IGameService gameService)
        {
            _sessionService = sessionService;
            _handService = handService;
            _gameService = gameService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession()
        {
            try
            {
                var session = await _sessionService.CreateSessionAsync();
                return Ok(new { id = session.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSessionById(string id)
        {
            try
            {
                var session = await _sessionService.GetSessionByIdAsync(id);

                if (session.Ended())
                {
                    return Ok(new { ended = true, evLoss = session.ExpectedValueLoss, statistics = session.Statistics });
                }

                var hand = await _sessionService.GetHandFromSessionAsync(session);

                var playerState = hand.CurrentPlayerState();
                var dealerState = hand.CurrentDealerState();

                var actions = await _gameService.GetActionsFromStatesAsync(playerState, dealerState);

                await HandleTerminalPlayerStateAsync(session, playerState);

                return Ok(new { ended = false, hand, actions, terminal = playerState.StateType == GameStateType.Terminal });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> MakeMoveForSession(string id, [FromBody] MakeMoveRequest request)
        {
            try
            {
                var session = await _sessionService.GetSessionByIdAsync(id);
                var hand = await _sessionService.GetHandFromSessionAsync(session);

                var move = request.Move;
                var label = request.Label;

                await _sessionService.UpdateSessionAnalyticsAsync(session, hand, move, label);

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

                await _handService.UpdateHandAsync(hand);
                await HandleTerminalPlayerStateAsync(session, playerState);

                var dealerState = hand.CurrentDealerState();

                Actions actions;

                try
                {
                    actions = await _gameService.GetActionsFromStatesAsync(playerState, dealerState);
                }
                catch
                {
                    actions = new Actions(null, null, null, null);
                }

                return Ok(new { ended = false, hand, actions, terminal = playerState.StateType == GameStateType.Terminal });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // [HttpDelete]
        // public async Task<IActionResult> DeleteSessions()
        // {
        //     try
        //     {
        //         await _sessionService.DeleteSessionsAsync();
        //         return Ok();
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new { message = ex.Message });
        //     }
        // }

        private async Task HandleTerminalPlayerStateAsync(Session session, PlayerState playerState)
        {
            if (playerState.StateType == GameStateType.Terminal)
            {
                session.NextHand();
            }

            await _sessionService.UpdateSessionHandIndexAsync(session);
        }
    }
}
