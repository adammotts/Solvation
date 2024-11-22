using System.Text.Json.Serialization;

namespace Solvation.Enums {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Suit {
        Spades,
        Hearts,
        Diamonds,
        Clubs
    }
}