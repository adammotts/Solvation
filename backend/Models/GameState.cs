using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Solvation.Enums;

namespace Solvation.Models
{
    public class GameState
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id;

        public int PlayerSumValue;

        public GameStateValueType PlayerValueType;

        public GameStateType PlayerStateType;
        public int DealerFaceUpValue;

        public GameStateValueType DealerValueType;

        public GameStateType DealerStateType;

        public Actions Actions = new Actions();
    }
}