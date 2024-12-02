using Solvation.Enums;

namespace Solvation.Requests
{
    public class GetGameStateRequest
    {
        public int PlayerSumValue;
        public GameStateValueType PlayerValueType;
        public GameStateType PlayerStateType;
        public int DealerSumValue;
        public GameStateValueType DealerValueType;
        public GameStateType DealerStateType;
    }
}
