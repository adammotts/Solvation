using Solvation.Enums;

namespace Solvation.Requests
{
    public class GetGameStateRequest
    {
        public int PlayerSumValue;
        public GameStateValueType PlayerValueType;
        public GameStateType PlayerStateType;
        public int DealerFaceUpValue;
        public GameStateValueType DealerValueType;
        public GameStateType DealerStateType;
    }
}
