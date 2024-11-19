using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Solvation.Enums;

namespace Solvation.Models {
    public class GameActions {
        public double? Hit { get; set; }
        public double? Stand { get; set; }
        public double? Double { get; set; }
        public double? Split { get; set; }
    }
}