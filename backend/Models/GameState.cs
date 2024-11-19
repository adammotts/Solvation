using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using Solvation.Enums;

namespace Solvation.Models {
    public class GameState {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int PlayerSumValue { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GameStateValueType PlayerValueType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GameStateType PlayerStateType { get; set; }

        public int DealerFaceUpValue { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GameStateValueType DealerValueType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GameStateType DealerStateType { get; set; }
    }
}