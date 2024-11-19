using System.Text.Json.Serialization;

namespace Solvation.Enums {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GameStateValueType {
        Soft,
        Hard,
        Blackjack,
        MaybeBlackjack,
        Splittable
    }
}