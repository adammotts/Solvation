using Solvation.Enums;

namespace Solvation.Models
{
    public class GetGameStateRequest
    {
        public int PlayerSumValue { get; set; }
        public GameStateValueType PlayerValueType { get; set; }
        public GameStateType PlayerStateType { get; set; }
        public int DealerFaceUpValue { get; set; }
        public GameStateValueType DealerValueType { get; set; }
        public GameStateType DealerStateType { get; set; }
    }
}
