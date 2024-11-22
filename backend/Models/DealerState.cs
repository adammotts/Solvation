using Solvation.Enums;

namespace Solvation.Models
{
    public class DealerState : PileState<DealerState>
    {
        public readonly bool Insurable;

        public DealerState(int sumValue, GameStateValueType valueType) : base(sumValue, valueType)
        {
            if (sumValue == 10 || sumValue == 11)
            {
                this.Insurable = true;
            }
            else
            {
                this.Insurable = false;
            }
        }

        public DealerState(int sumValue, GameStateValueType valueType, bool insurable) : base(sumValue, valueType)
        {
            this.Insurable = insurable;
        }

        protected override GameStateType DetermineStateType()
        {
            return this.SumValue >= 17 ? GameStateType.Terminal : GameStateType.Active;
        }

        public override DealerState Hit(Card card)
        {
            DealerState notBlackjack = this.AddCard(card);

            DealerState other = PileState<DealerState>.RankValues[card.Rank];

            if (this.Insurable && other.Insurable && notBlackjack.SumValue == 21)
            {
                return new DealerState(21, GameStateValueType.Blackjack, false);
            }

            return new DealerState(notBlackjack.SumValue, notBlackjack.ValueType, false);
        }

        public new static List<DealerState> AllStates()
        {
            var dealerStates = new List<DealerState>();

            for (int i = 26; i >= 22; i--)
            {
                dealerStates.Add(new DealerState(i, GameStateValueType.Hard, false));
            }

            dealerStates.Add(new DealerState(21, GameStateValueType.Blackjack, false));

            for (int i = 21; i >= 11; i--)
            {
                dealerStates.Add(new DealerState(i, GameStateValueType.Hard, false));
            }

            for (int i = 21; i >= 12; i--)
            {
                dealerStates.Add(new DealerState(i, GameStateValueType.Soft, false));
            }

            dealerStates.Add(new DealerState(11, GameStateValueType.Soft, true));
            dealerStates.Add(new DealerState(10, GameStateValueType.Hard, true));

            for (int i = 10; i >= 2; i--)
            {
                dealerStates.Add(new DealerState(i, GameStateValueType.Hard, false));
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
