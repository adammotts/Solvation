using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Solvation.Enums;

namespace Solvation.Models
{
    public class GameState
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int PlayerSumValue { get; set; }

        public GameStateValueType PlayerValueType { get; set; }

        public GameStateType PlayerStateType { get; set; }

        public bool PlayerDoubleable { get; set; }

        public bool PlayerSplittable { get; set; }

        public int DealerFaceUpValue { get; set; }

        public GameStateValueType DealerValueType { get; set; }

        public GameStateType DealerStateType { get; set; }

        public bool DealerInsurable { get; set; }

        public Actions Actions { get; set; } = new Actions(0, 0, 0, 0);

        public GameState(
            int playerSumValue,
            GameStateValueType playerValueType,
            GameStateType playerStateType,
            bool playerDoubleable,
            bool playerSplittable,
            int dealerFaceUpValue,
            GameStateValueType dealerValueType,
            GameStateType dealerStateType,
            bool dealerInsurable,
            Actions actions
        )
        {
            PlayerSumValue = playerSumValue;
            PlayerValueType = playerValueType;
            PlayerStateType = playerStateType;
            PlayerDoubleable = playerDoubleable;
            PlayerSplittable = playerSplittable;
            DealerFaceUpValue = dealerFaceUpValue;
            DealerValueType = dealerValueType;
            DealerStateType = dealerStateType;
            DealerInsurable = dealerInsurable;
            Actions = actions;
        }
    }
}