using Solvation.Enums;

namespace Solvation.Models
{
    public class PlayerState
    {
        public readonly int SumValue;

        public readonly GameStateValueType ValueType;

        public readonly GameStateType StateType;

        public readonly bool Splittable;

        public static readonly Dictionary<Rank, PlayerState> RankValues = new Dictionary<Rank, PlayerState>
        {
            { Rank.Two, new PlayerState(2, GameStateValueType.Hard) },
            { Rank.Three, new PlayerState(3, GameStateValueType.Hard) },
            { Rank.Four, new PlayerState(4, GameStateValueType.Hard) },
            { Rank.Five, new PlayerState(5, GameStateValueType.Hard) },
            { Rank.Six, new PlayerState(6, GameStateValueType.Hard) },
            { Rank.Seven, new PlayerState(7, GameStateValueType.Hard) },
            { Rank.Eight, new PlayerState(8, GameStateValueType.Hard) },
            { Rank.Nine, new PlayerState(9, GameStateValueType.Hard) },
            { Rank.Ten, new PlayerState(10, GameStateValueType.Hard) },
            { Rank.Jack, new PlayerState(10, GameStateValueType.Hard) },
            { Rank.Queen, new PlayerState(10, GameStateValueType.Hard) },
            { Rank.King, new PlayerState(10, GameStateValueType.Hard) },
            { Rank.Ace, new PlayerState(11, GameStateValueType.Soft) }
        };

        public PlayerState(int SumValue, GameStateValueType ValueType, bool Splittable = false)
        {
            this.SumValue = SumValue;
            this.ValueType = ValueType;
            if (this.SumValue >= 21)
            {
                this.StateType = GameStateType.Terminal;
            }
            else
            {
                this.StateType = GameStateType.Active;
            }
            this.Splittable = Splittable;
        }

        public PlayerState Hit(Card card)
        {
            if (this.StateType == GameStateType.Terminal)
            {
                throw new InvalidOperationException("Cannot act on terminal state");
            }

            PlayerState other = PlayerState.RankValues[card.Rank];

            int thisValue = this.SumValue;
            GameStateValueType thisValueType = this.ValueType;
            int otherValue = other.SumValue;
            GameStateValueType otherValueType = other.ValueType;

            int resultValue = thisValue + otherValue;
            GameStateValueType resultValueType;

            if (otherValue == 11 && otherValueType == GameStateValueType.Soft)
            {
                if (resultValue > 21)
                {
                    resultValue -= 10;
                    resultValueType = thisValueType;
                }
                else
                {
                    resultValueType = GameStateValueType.Soft;
                }
            }
            else
            {
                if (thisValueType == GameStateValueType.Hard)
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

            return new PlayerState(resultValue, resultValueType);
        }

        public PlayerState Split()
        {
            if (this.StateType == GameStateType.Terminal)
            {
                throw new InvalidOperationException("Cannot act on terminal state");
            }

            if (!this.Splittable)
            {
                throw new ArgumentException("Not Splittable");
            }

            int splitValue;
            GameStateValueType splitValueType;

            if (this.ValueType == GameStateValueType.Soft)
            {
                splitValue = 11;
                splitValueType = GameStateValueType.Soft;
            }
            else
            {
                splitValue = this.SumValue / 2;
                splitValueType = GameStateValueType.Hard;
            }

            return new PlayerState(splitValue, splitValueType);
        }

        public static List<PlayerState> AllPlayerStates()
        {
            List<PlayerState> playerStates = new List<PlayerState>();

            for (int i = 30; i >= 22; i--)
            {
                playerStates.Add(new PlayerState(i, GameStateValueType.Hard));
            }

            playerStates.Add(new PlayerState(21, GameStateValueType.Blackjack));

            for (int i = 21; i >= 11; i--)
            {
                playerStates.Add(new PlayerState(i, GameStateValueType.Hard));
            }

            for (int i = 21; i >= 11; i--)
            {
                playerStates.Add(new PlayerState(i, GameStateValueType.Soft));
            }

            for (int i = 10; i >= 4; i--)
            {
                playerStates.Add(new PlayerState(i, GameStateValueType.Hard));
            }

            playerStates.Add(new PlayerState(12, GameStateValueType.Soft, true));

            for (int i = 10; i >= 2; i--)
            {
                playerStates.Add(new PlayerState(i * 2, GameStateValueType.Hard, true));
            }

            return playerStates;
        }
    }
}