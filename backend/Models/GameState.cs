using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Solvation.Enums;

namespace Solvation.Models {
    public class GameState {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int PlayerSumValue { get; set; }

        public GameStateValueType PlayerValueType { get; set; }

        public GameStateType PlayerStateType { get; set; }

        public int DealerFaceUpValue { get; set; }

        public GameStateValueType DealerValueType { get; set; }

        public GameStateType DealerStateType { get; set; }

        public GameActions Actions { get; set; } = new GameActions();
    }
}