using System.Text;
using Solvation.Enums;

namespace Solvation.Models
{
    public class DealerState : PileState
    {
        // This is an alias for MaybeBlackjack. If I want to implement insurance logic, then this needs to be changed for 10s, since they are not insurable
        public bool Insurable { get; private set; }

        public new readonly static Dictionary<Rank, DealerState> RankValues = new Dictionary<Rank, DealerState>
        {
            { Rank.Two, new DealerState(2, GameStateValueType.Hard) },
            { Rank.Three, new DealerState(3, GameStateValueType.Hard) },
            { Rank.Four, new DealerState(4, GameStateValueType.Hard) },
            { Rank.Five, new DealerState(5, GameStateValueType.Hard) },
            { Rank.Six, new DealerState(6, GameStateValueType.Hard) },
            { Rank.Seven, new DealerState(7, GameStateValueType.Hard) },
            { Rank.Eight, new DealerState(8, GameStateValueType.Hard) },
            { Rank.Nine, new DealerState(9, GameStateValueType.Hard) },
            { Rank.Ten, new DealerState(10, GameStateValueType.Hard, true) },
            { Rank.Jack, new DealerState(10, GameStateValueType.Hard, true) },
            { Rank.Queen, new DealerState(10, GameStateValueType.Hard, true) },
            { Rank.King, new DealerState(10, GameStateValueType.Hard, true) },
            { Rank.Ace, new DealerState(11, GameStateValueType.Soft, true) }
        };

        public DealerState(int sumValue, GameStateValueType valueType, bool insurable = false) : base(sumValue, valueType)
        {
            this.Insurable = insurable;
        }

        protected override GameStateType DetermineStateType()
        {
            return this.SumValue >= 17 ? GameStateType.Terminal : GameStateType.Active;
        }

        // "Hit" refers to dealer adding cards after the hidden card is revealed (blackjack is impossible)
        public override DealerState Hit(Card card)
        {
            DealerState other = DealerState.RankValues[card.Rank];
            DealerState.Combine(this, other, out int resultValue, out GameStateValueType resultValueType);

            if (this.Insurable && other.Insurable && resultValue == 21)
            {
                throw new InvalidOperationException("Blackjack is not possible on hit");
            }

            return new DealerState(resultValue, resultValueType);
        }

        // "Reveal" refers to dealer revealing the hidden card (blackjack is possible)
        public DealerState Reveal(Card card)
        {
            DealerState other = DealerState.RankValues[card.Rank];
            DealerState.Combine(this, other, out int resultValue, out GameStateValueType resultValueType);

            if (this.Insurable && other.Insurable && resultValue == 21)
            {
                return new DealerState(21, GameStateValueType.Blackjack);
            }

            return new DealerState(resultValue, resultValueType);
        }

        public new static DealerState[] AllStates()
        {
            var dealerStates = new List<DealerState>();

            for (int i = 26; i >= 22; i--)
            {
                dealerStates.Add(new DealerState(i, GameStateValueType.Hard));
            }

            dealerStates.Add(new DealerState(21, GameStateValueType.Blackjack));

            for (int i = 21; i >= 11; i--)
            {
                dealerStates.Add(new DealerState(i, GameStateValueType.Hard));
            }

            for (int i = 21; i >= 12; i--)
            {
                dealerStates.Add(new DealerState(i, GameStateValueType.Soft));
            }

            // Represent both pre-reveal (blackjack is possible) and post-reveal cards (blackjack is impossible)
            dealerStates.Add(new DealerState(11, GameStateValueType.Soft, true));
            dealerStates.Add(new DealerState(10, GameStateValueType.Hard, true));

            for (int i = 10; i >= 2; i--)
            {
                dealerStates.Add(new DealerState(i, GameStateValueType.Hard));
            }

            return dealerStates.ToArray();
        }

        public new static DealerState[] AllTerminalStates()
        {
            var terminalStates = new List<DealerState>();

            foreach (DealerState dealerState in DealerState.AllStates())
            {
                if (dealerState.StateType == GameStateType.Terminal)
                {
                    terminalStates.Add(dealerState);
                }
            }

            return terminalStates.ToArray();
        }

        public new static string Interactions()
        {
            StringBuilder result = new StringBuilder();

            foreach (DealerState dealerState in DealerState.AllStates())
            {
                foreach (Card card in Card.AllRanks())
                {
                    try {
                        DealerState afterHit = dealerState.Hit(card);
                        result.AppendLine($"{dealerState} + {DealerState.RankValues[card.Rank]} = {afterHit}");
                    }
                    catch
                    {
                        result.AppendLine($"{dealerState} + {DealerState.RankValues[card.Rank]} = INVALID");
                    }
                }
            }

            return result.ToString();
        }

        public static string RevealInteractions()
        {
            StringBuilder result = new StringBuilder();

            foreach (Card startingCard in Card.AllRanks())
            {
                foreach (Card card in Card.AllRanks())
                {
                    DealerState dealerState = DealerState.RankValues[startingCard.Rank];

                    try {
                        DealerState afterReveal = dealerState.Reveal(card);
                        result.AppendLine($"{startingCard} + {card} = {afterReveal}");
                    }
                    catch
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            return result.ToString();
        }

        public static DealerState FromCard(Card card)
        {
            return DealerState.RankValues[card.Rank];
        }

        public override bool Equals(object? obj)
        {
            if (obj is DealerState other)
            {
                return this.SumValue == other.SumValue && this.ValueType == other.ValueType && this.StateType == other.StateType && this.Insurable == other.Insurable;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.SumValue, this.ValueType, this.StateType, this.Insurable);
        }

        public override string ToString()
        {
            return $"[{this.SumValue}, {this.ValueType}, {this.StateType}, {(this.Insurable ? "Insurable" : "Not Insurable")}]";
        }
    }
}
