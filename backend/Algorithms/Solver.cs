using Solvation.Models;
using Solvation.Enums;

namespace Solvation.Algorithms
{
    public class Solver
    {
        public static GameState[] Solve()
        {
            List<GameState> gameStates = new List<GameState>();

            foreach (PlayerState playerState in PlayerState.AllStates())
            {
                foreach (DealerState dealerState in DealerState.AllStates())
                {
                    gameStates.Add(new GameState(
                        playerState.SumValue,
                        playerState.ValueType,
                        playerState.StateType,
                        dealerState.SumValue,
                        dealerState.ValueType,
                        dealerState.StateType
                    ));
                }
            }

            return gameStates.ToArray();
        }
    }
}