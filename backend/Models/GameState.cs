using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Solvation.Enums;

namespace Solvation.Models
{
    public class GameState
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; }

        public int PlayerSumValue { get; }

        public GameStateValueType PlayerValueType { get; }

        public GameStateType PlayerStateType { get; }
        public int DealerFaceUpValue { get; }

        public GameStateValueType DealerValueType { get; }

        public GameStateType DealerStateType { get; }

        public Actions Actions { get; } = new Actions();

        public GameState(
            int playerSumValue,
            GameStateValueType playerValueType,
            GameStateType playerStateType,
            int dealerFaceUpValue,
            GameStateValueType dealerValueType,
            GameStateType dealerStateType
        )
        {
            PlayerSumValue = playerSumValue;
            PlayerValueType = playerValueType;
            PlayerStateType = playerStateType;
            DealerFaceUpValue = dealerFaceUpValue;
            DealerValueType = dealerValueType;
            DealerStateType = dealerStateType;
        }
    }
}