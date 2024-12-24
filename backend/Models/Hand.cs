using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Solvation.Enums;

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

            while (this.CurrentPlayerState().ValueType == GameStateValueType.Blackjack)
            {
                this.PlayerCards.RemoveAt(1);
                this.PlayerCards.Add(Card.Deal());
            }
        }

        public DealerState CurrentDealerState()
        {
            return DealerState.FromCard(this.DealerCards.First());
        }

        public PlayerState CurrentPlayerState()
        {
            return PlayerState.FromCards(this.PlayerCards.ToArray());
        }

        public PlayerState Hit()
        {
            this.PlayerCards.Add(Card.Deal());
            return PlayerState.FromCards(this.PlayerCards.ToArray());
        }

        public PlayerState Stand()
        {
            return PlayerState.FromCards(this.PlayerCards.ToArray(), overrideIsTerminal: true);
        }

        public PlayerState Double()
        {
            this.PlayerCards.Add(Card.Deal());
            return PlayerState.FromCards(this.PlayerCards.ToArray(), overrideIsTerminal: true);
        }

        public PlayerState Split()
        {
            this.PlayerCards.RemoveAt(1);
            this.PlayerCards.Add(Card.Deal());
            return PlayerState.FromCards(this.PlayerCards.ToArray(), overrideIsNotSplittable: true);
        }

        public override string ToString()
        {
            return $"[ID: {Id}, Player: {PlayerCards}, Dealer: {DealerCards}]";
        }
    }
}