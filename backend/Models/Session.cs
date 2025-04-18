using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Solvation.Models
{
    public class Session
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        public List<string?> HandIds { get; set; } = [];

        public int CurrentHandIndex { get; set; } = 0;

        public double ExpectedValueLoss { get; set; } = 0.0;

        public Statistics Statistics { get; set; } = new Statistics();
        
        public Session(Hand[] hands)
        {
            this.CurrentHandIndex = 0;
            this.HandIds = new List<string?>();

            foreach (Hand hand in hands)
            {
                this.HandIds.Add(hand.Id);
            }
        }

        public string? CurrentHandId()
        {
            return this.HandIds[this.CurrentHandIndex];
        }

        public bool Ended()
        {
            return this.CurrentHandIndex >= this.HandIds.Count;
        }

        public void NextHand()
        {
            this.CurrentHandIndex++;
        }

        public override string ToString()
        {
            return $"[ID: {Id}, Current Index: {CurrentHandIndex}, Hand IDs: {HandIds}]";
        }
    }
}