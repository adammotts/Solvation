using System.Text.Json.Serialization;

namespace Solvation.Enums {
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GameStateType {
        Active,
        Terminal
    }
}