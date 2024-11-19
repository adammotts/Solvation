using Solvation.Enums;
using Solvation.Models;

namespace Solvation.Requests
{
    public class GenerateGameStateRequest
    {
        public int PlayerSumValue { get; set; }
        public GameStateValueType PlayerValueType { get; set; }
        public GameStateType PlayerStateType { get; set; }
        public int DealerFaceUpValue { get; set; }
        public GameStateValueType DealerValueType { get; set; }
        public GameStateType DealerStateType { get; set; }
        public GameActionsRequest Actions { get; set; } = new GameActionsRequest();
    }
}