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

        private static T CreateState(int sumValue, GameStateValueType valueType)
        {
            return (T)Activator.CreateInstance(typeof(T), sumValue, valueType)!;
        }

        protected PileState(int sumValue, GameStateValueType valueType)
        {
            this.SumValue = sumValue;
            this.ValueType = valueType;
            this.StateType = this.DetermineStateType();
        }

        protected abstract GameStateType DetermineStateType();

        public abstract T Hit(Card card);

        protected T Combine(T other)
        {
            if (this.StateType == GameStateType.Terminal)
                throw new InvalidOperationException("Cannot act on terminal state");

            int resultValue = this.SumValue + other.SumValue;
            GameStateValueType resultValueType;

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

            return PileState<T>.CreateState(resultValue, resultValueType);
        }

        public static List<T> AllStates()
        {
            throw new NotImplementedException();
        }

        public static List<T> AllTerminalStates()
        {
            throw new NotImplementedException();
        }
    }
}