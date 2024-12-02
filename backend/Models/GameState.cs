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

        public PlayerState PlayerState { get; set; } = new PlayerState(0, GameStateValueType.Hard);

        public DealerState DealerState { get; set; } = new DealerState(0, GameStateValueType.Hard);

        public Actions Actions { get; set; } = new Actions(0, 0, 0, 0);
        
        public GameState() {}

        public GameState(
            PlayerState playerState,
            DealerState dealerState,
            Actions actions
        )
        {
            this.PlayerState = playerState;
            this.DealerState = dealerState;
            this.Actions = actions;
        }

        public override string ToString()
        {
            return $"[Player: {PlayerState}, Dealer: {DealerState}, Actions: {Actions}]";
        }
    }
}