using Solvation.Enums;

namespace Solvation.Models
{
    public abstract class PileState
    {
        public readonly int SumValue;
        public readonly GameStateValueType ValueType;
        public readonly GameStateType StateType;

        public readonly static Dictionary<Rank, PlayerState> RankValues = new Dictionary<Rank, PlayerState>();

        protected PileState(int sumValue, GameStateValueType valueType)
        {
            this.SumValue = sumValue;
            this.ValueType = valueType;
            this.StateType = this.DetermineStateType();
        }

        protected abstract GameStateType DetermineStateType();

        public abstract PileState Hit(Card card);

        protected static void Combine(PileState first, PileState second, out int resultValue, out GameStateValueType resultValueType)
        {
            if (first.StateType == GameStateType.Terminal || second.StateType == GameStateType.Terminal)
                throw new InvalidOperationException("Cannot act on terminal state");

            resultValue = first.SumValue + second.SumValue;
            resultValueType = first.ValueType;

            if (second.SumValue == 11 && second.ValueType == GameStateValueType.Soft)
            {
                if (resultValue > 21)
                {
                    resultValue -= 10;
                }
                else
                {
                    resultValueType = GameStateValueType.Soft;
                }
            }
            else
            {
                if (first.ValueType == GameStateValueType.Hard)
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
        }

        public static PileState[] AllStates()
        {
            throw new NotImplementedException();
        }

        public static PileState[] AllTerminalStates()
        {
            throw new NotImplementedException();
        }

        public static string Interactions()
        {
            throw new NotImplementedException();
        }
    }
}
