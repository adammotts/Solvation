using System.Text.Json.Serialization;

namespace Solvation.Enums {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Rank {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
}