using Solvation.Enums;

namespace Solvation.Models
{
    public abstract class PileState<T> where T : PileState<T>
    {
        public readonly int SumValue;
        public readonly GameStateValueType ValueType;
        public readonly GameStateType StateType;
        public readonly static Dictionary<Rank, T> RankValues = new Dictionary<Rank, T>
        {
            { Rank.Two, CreateState(2, GameStateValueType.Hard) },
            { Rank.Three, CreateState(3, GameStateValueType.Hard) },
            { Rank.Four, CreateState(4, GameStateValueType.Hard) },
            { Rank.Five, CreateState(5, GameStateValueType.Hard) },
            { Rank.Six, CreateState(6, GameStateValueType.Hard) },
            { Rank.Seven, CreateState(7, GameStateValueType.Hard) },
            { Rank.Eight, CreateState(8, GameStateValueType.Hard) },
            { Rank.Nine, CreateState(9, GameStateValueType.Hard) },
            { Rank.Ten, CreateState(10, GameStateValueType.Hard) },
            { Rank.Jack, CreateState(10, GameStateValueType.Hard) },
            { Rank.Queen, CreateState(10, GameStateValueType.Hard) },
            { Rank.King, CreateState(10, GameStateValueType.Hard) },
            { Rank.Ace, CreateState(11, GameStateValueType.Soft) }
        };

        protected PileState(int sumValue, GameStateValueType valueType)
        {
            this.SumValue = sumValue;
            this.ValueType = valueType;
            this.StateType = this.DetermineStateType();
        }

        protected abstract GameStateType DetermineStateType();

        public abstract T Hit(Card card);

        protected T AddCard(Card card)
        {
            if (this.StateType == GameStateType.Terminal)
                throw new InvalidOperationException("Cannot act on terminal state");

            T other = PileState<T>.RankValues[card.Rank];

            int resultValue = this.SumValue + other.SumValue;
            GameStateValueType resultValueType = this.DetermineValueType(resultValue, other);

            return PileState<T>.CreateState(resultValue, resultValueType);
        }

        protected static T CreateState(int sumValue, GameStateValueType valueType)
        {
            return (T)Activator.CreateInstance(typeof(T), sumValue, valueType)!;
        }

        private GameStateValueType DetermineValueType(int resultValue, T other)
        {
            if (other.SumValue == 11 && other.ValueType == GameStateValueType.Soft)
            {
                if (resultValue > 21)
                    return this.ValueType;
                return GameStateValueType.Soft;
            }

            if (this.ValueType == GameStateValueType.Hard)
                return GameStateValueType.Hard;

            if (resultValue > 21)
                return GameStateValueType.Hard;

            return GameStateValueType.Soft;
        }

        public static List<T> AllStates()
        {
            throw new NotImplementedException();
        }
    }
}
