using Solvation.Enums;

namespace Solvation.Models
{
    public class Card
    {
        public readonly Rank Rank;

        public readonly Suit Suit;

        public Card(Rank rank, Suit suit)
        {
            this.Rank = rank;
            this.Suit = suit;
        }

        public readonly static Dictionary<Rank, string> Ranks = new Dictionary<Rank, string>
        {
            { Rank.Two, "2" },
            { Rank.Three, "3" },
            { Rank.Four, "4" },
            { Rank.Five, "5" },
            { Rank.Six, "6" },
            { Rank.Seven, "7" },
            { Rank.Eight, "8" },
            { Rank.Nine, "9" },
            { Rank.Ten, "10" },
            { Rank.Jack, "J" },
            { Rank.Queen, "Q" },
            { Rank.King, "K" },
            { Rank.Ace, "A" }
        };

        public readonly static Dictionary<Suit, string> Suits = new Dictionary<Suit, string>
        {
            { Suit.Spades, "♠" },
            { Suit.Hearts, "♥" },
            { Suit.Diamonds, "♦" },
            { Suit.Clubs, "♣" }
        };

        public static Card[] Deck()
        {
            List<Card> deck = new List<Card>();

            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    deck.Add(new Card(rank, suit));
                }
            }

            return deck.ToArray();
        }

        public static Card[] AllRanks()
        {
            List<Card> allRanks = new List<Card>();

            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                allRanks.Add(new Card(rank, Suit.Spades));
            }

            return allRanks.ToArray();
        }

        public override bool Equals(object? obj)
        {
            if (obj is Card other)
            {
                return this.Rank == other.Rank && this.Suit == other.Suit;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Rank, this.Suit);
        }

        public override string ToString()
        {
            return $"{Ranks[this.Rank]}{Suits[this.Suit]}";
        }
    }
}