using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Solvation.Enums;

namespace Solvation.Models
{
    public class GameActions
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        public double Play { get; set; }

        public double Abstain { get; set; }
        
        public GameActions() {}

        public GameActions(double play)
        {
            this.Play = play;
            this.Abstain = 0;
        }

        public override string ToString()
        {
            return $"[Play: {Play}, Abstain: {Abstain}]";
        }
    }
}