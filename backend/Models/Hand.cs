using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Solvation.Models
{
    public class Hand
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        public List<Card> PlayerCards { get; set; } = [];

        public List<Card> DealerCards { get; set; } = [];
        
        public Hand()
        {
            this.PlayerCards = new List<Card>();
            this.DealerCards = new List<Card>();

            this.PlayerCards.Add(Card.Deal());
            this.DealerCards.Add(Card.Deal());
            this.PlayerCards.Add(Card.Deal());
            this.DealerCards.Add(Card.Deal());
        }

        public override string ToString()
        {
            return $"[ID: {Id}, Player: {PlayerCards}, Dealer: {DealerCards}]";
        }
    }
}