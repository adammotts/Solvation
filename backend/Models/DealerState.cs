using Solvation.Enums;

namespace Solvation.Models
{
    public class DealerState
    {
        public readonly int SumValue;
        public readonly GameStateValueType ValueType;

        public readonly GameStateType StateType;

        public readonly bool Insurable;

        public static readonly Dictionary<Rank, DealerState> RankValues = new Dictionary<Rank, DealerState>
        {
            { Rank.Two, new DealerState(2, GameStateValueType.Hard) },
            { Rank.Three, new DealerState(3, GameStateValueType.Hard) },
            { Rank.Four, new DealerState(4, GameStateValueType.Hard) },
            { Rank.Five, new DealerState(5, GameStateValueType.Hard) },
            { Rank.Six, new DealerState(6, GameStateValueType.Hard) },
            { Rank.Seven, new DealerState(7, GameStateValueType.Hard) },
            { Rank.Eight, new DealerState(8, GameStateValueType.Hard) },
            { Rank.Nine, new DealerState(9, GameStateValueType.Hard) },
            { Rank.Ten, new DealerState(10, GameStateValueType.Hard) },
            { Rank.Jack, new DealerState(10, GameStateValueType.Hard) },
            { Rank.Queen, new DealerState(10, GameStateValueType.Hard) },
            { Rank.King, new DealerState(10, GameStateValueType.Hard) },
            { Rank.Ace, new DealerState(11, GameStateValueType.Soft) }
        };

        public DealerState(int SumValue, GameStateValueType ValueType, bool Insurable = false)
        {
            this.SumValue = SumValue;
            this.ValueType = ValueType;
            if (this.SumValue >= 17)
            {
                this.StateType = GameStateType.Terminal;
            }
            else
            {
                this.StateType = GameStateType.Active;
            }
            this.Insurable = Insurable;
        }

        public DealerState Hit(Card card)
        {
            if (this.StateType == GameStateType.Terminal)
            {
                throw new InvalidOperationException("Cannot act on terminal state");
            }

            DealerState other = DealerState.RankValues[card.Rank];

            int resultValue = this.SumValue + other.SumValue;
            GameStateValueType resultValueType;

            if (this.Insurable && other.Insurable && resultValue == 21)
            {
                return new DealerState(21, GameStateValueType.Blackjack);
            }

            if (other.SumValue == 11 && other.ValueType == GameStateValueType.Soft)
            {
                if (resultValue > 21)
                {
                    resultValue -= 10;
                    resultValueType = this.ValueType;
                }
                else
                {
                    resultValueType = GameStateValueType.Soft;
                }
            }
            else
            {
                if (this.ValueType == GameStateValueType.Hard)
                {
                    resultValueType = GameStateValueType.Hard;
                }
                else
                {
                    if (resultValue > 21)
                    {
                        resultValue -= 10;
                        resultValueType = GameStateValueType.Hard;
                    }
                    else
                    {
                        resultValueType = GameStateValueType.Soft;
                    }
                }
            }

            return new DealerState(resultValue, resultValueType);
        }

        public static List<DealerState> AllDealerStates()
        {
            List<DealerState> dealerStates = new List<DealerState>();

            for (int i = 26; i >= 22; i--) {
                dealerStates.Add(new DealerState(i, GameStateValueType.Hard));
            }

            dealerStates.Add(new DealerState(21, GameStateValueType.Blackjack));

            for (int i = 21; i >= 11; i--) {
                dealerStates.Add(new DealerState(i, GameStateValueType.Hard));
            }

            for (int i = 21; i >= 12; i--) {
                dealerStates.Add(new DealerState(i, GameStateValueType.Soft));
            }

            dealerStates.Add(new DealerState(11, GameStateValueType.Soft, true));
            dealerStates.Add(new DealerState(10, GameStateValueType.Hard, true));

            for (int i = 10; i >= 2; i--) {
                dealerStates.Add(new DealerState(i, GameStateValueType.Hard));
            }

            return dealerStates;
        }
    }
}