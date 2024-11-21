using Solvation.Models;
using Solvation.Enums;

namespace Solvation.Algorithms
{
    public class Solver
    {
        public static GameState[] Solve()
        {
            var gameState = new GameState
            {
                PlayerSumValue = 21,
                PlayerValueType = GameStateValueType.Blackjack,
                PlayerStateType = GameStateType.Terminal,
                DealerFaceUpValue = 10,
                DealerValueType = GameStateValueType.Hard,
                DealerStateType = GameStateType.Active,
                Actions = new GameActions
                {
                    Hit = null,
                    Stand = 0.85,
                    Double = 1.25,
                    Split = 0.95
                }
            };

            return new[] { gameState };
        }
    }
}