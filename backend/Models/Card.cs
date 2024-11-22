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
    }
}