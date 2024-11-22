using Solvation.Enums;

namespace Solvation.Models
{
    public class DealerState : PileState
    {
        public readonly bool Insurable;

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

        public override DealerState Hit(Card card)
        {
            DealerState other = DealerState.RankValues[card.Rank];
            PileState.Combine(this, other, out int resultValue, out GameStateValueType resultValueType);

            if (this.Insurable && other.Insurable && resultValue == 21)
            {
                return new DealerState(21, GameStateValueType.Blackjack);
            }

            return new DealerState(resultValue, resultValueType);
        }

        public new static List<DealerState> AllStates()
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

            dealerStates.Add(new DealerState(11, GameStateValueType.Soft, true));
            dealerStates.Add(new DealerState(10, GameStateValueType.Hard, true));

            for (int i = 10; i >= 2; i--)
            {
                dealerStates.Add(new DealerState(i, GameStateValueType.Hard));
            }

            return dealerStates;
        }

        public new static List<DealerState> AllTerminalStates()
        {
            var terminalStates = new List<DealerState>();

            foreach (DealerState dealerState in DealerState.AllStates())
            {
                if (dealerState.StateType == GameStateType.Terminal)
                {
                    terminalStates.Add(dealerState);
                }
            }

            return terminalStates;
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
