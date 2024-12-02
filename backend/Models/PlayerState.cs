using System.Text;
using Solvation.Enums;

namespace Solvation.Models
{
    public class PlayerState : PileState
    {
        public bool Doubleable { get; private set; }

        public bool Splittable { get; private set; }

        public new readonly static Dictionary<Rank, PlayerState> RankValues = new Dictionary<Rank, PlayerState>
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

        public PlayerState(int sumValue, GameStateValueType valueType, bool doubleable = false, bool splittable = false) : base(sumValue, valueType)
        {
            if (splittable && !doubleable)
                throw new ArgumentException("Splittable implies Doubleable");

            this.Doubleable = doubleable;
            this.Splittable = splittable;
        }

        protected override GameStateType DetermineStateType()
        {
            return this.SumValue >= 21 ? GameStateType.Terminal : GameStateType.Active;
        }

        public override PlayerState Hit(Card card)
        {
            PlayerState other = PlayerState.RankValues[card.Rank];
            PlayerState.Combine(this, other, out int resultValue, out GameStateValueType resultValueType);
            return new PlayerState(resultValue, resultValueType);
        }

        public PlayerState Split(Card card)
        {
            if (this.StateType == GameStateType.Terminal)
                throw new InvalidOperationException("Cannot act on terminal state");

            if (!this.Splittable)
                throw new ArgumentException("Not Splittable");

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

            PlayerState resultAfterSplit = new PlayerState(splitValue, splitValueType);

            PlayerState other = PlayerState.RankValues[card.Rank];
            PlayerState.Combine(resultAfterSplit, other, out int resultValue, out GameStateValueType resultValueType);

            return new PlayerState(resultValue, resultValueType, true);
        }

        public PlayerState DeclineSplit()
        {
            if (!this.Splittable)
                throw new ArgumentException("Not Splittable");

            return new PlayerState(this.SumValue, this.ValueType, this.Doubleable, false);
        }

        public static PlayerState FromCards(Card[] cards)
        {
            if (cards.Length < 1)
            {
                throw new ArgumentException("At least one card is required");
            }

            PlayerState playerState = PlayerState.RankValues[cards[0].Rank];

            for (int i = 1; i < cards.Length; i++)
            {
                playerState = playerState.Hit(cards[i]);
            }

            bool doubleable = false;
            bool splittable = false;

            if (cards.Length == 2)
            {
                doubleable = true;

                if (PlayerState.RankValues[cards[0].Rank].SumValue == PlayerState.RankValues[cards[1].Rank].SumValue)
                {
                    splittable = true;
                }
            }

            return new PlayerState(playerState.SumValue, playerState.ValueType, doubleable, splittable);
        }

        public new static PlayerState[] AllStates()
        {
            List<PlayerState> playerStates = new List<PlayerState>();

            for (int i = 30; i >= 22; i--)
            {
                playerStates.Add(new PlayerState(i, GameStateValueType.Hard));
            }

            playerStates.Add(new PlayerState(21, GameStateValueType.Blackjack));
            playerStates.Add(new PlayerState(21, GameStateValueType.Hard));
            playerStates.Add(new PlayerState(21, GameStateValueType.Soft));
            playerStates.Add(new PlayerState(21, GameStateValueType.Hard, true));
            playerStates.Add(new PlayerState(21, GameStateValueType.Soft, true));

            for (int i = 20; i >= 11; i--)
            {
                playerStates.Add(new PlayerState(i, GameStateValueType.Hard));
            }

            for (int i = 20; i >= 11; i--)
            {
                playerStates.Add(new PlayerState(i, GameStateValueType.Soft));
            }

            for (int i = 10; i >= 2; i--)
            {
                playerStates.Add(new PlayerState(i, GameStateValueType.Hard));
            }

            /* Doubleable */
            for (int i = 20; i >= 11; i--)
            {
                playerStates.Add(new PlayerState(i, GameStateValueType.Hard, true));
            }

            for (int i = 20; i >= 11; i--)
            {
                playerStates.Add(new PlayerState(i, GameStateValueType.Soft, true));
            }

            for (int i = 10; i >= 2; i--)
            {
                playerStates.Add(new PlayerState(i, GameStateValueType.Hard, true));
            }

            /* Splittable */
            playerStates.Add(new PlayerState(12, GameStateValueType.Soft, true, true));

            for (int i = 10; i >= 2; i--)
            {
                playerStates.Add(new PlayerState(i * 2, GameStateValueType.Hard, true, true));
            }

            return playerStates.ToArray();
        }

        public new static string Interactions()
        {
            StringBuilder result = new StringBuilder();

            foreach (PlayerState playerState in PlayerState.AllStates())
            {
                foreach (Card card in Card.AllRanks())
                {
                    try {
                        PlayerState afterHit = playerState.Hit(card);
                        result.AppendLine($"{playerState} + {PlayerState.RankValues[card.Rank]} = {afterHit}");
                    }
                    catch
                    {
                        result.AppendLine($"{playerState} + {PlayerState.RankValues[card.Rank]} = INVALID");
                    }
                }
            }

            return result.ToString();
        }

        public static string SplitInteractions()
        {
            StringBuilder result = new StringBuilder();

            foreach (PlayerState playerState in PlayerState.AllStates())
            {
                foreach (Card card in Card.AllRanks())
                {
                    try {
                        PlayerState afterSplit = playerState.Split(card);
                        result.AppendLine($"Split {playerState} + {PlayerState.RankValues[card.Rank]} = {afterSplit}");
                    }
                    catch
                    {
                        result.AppendLine($"Split {playerState} + {PlayerState.RankValues[card.Rank]} = INVALID");
                    }
                }
            }

            return result.ToString();
        }
        
        public override bool Equals(object? obj)
        {
            if (obj is PlayerState other)
            {
                return
                    this.SumValue == other.SumValue &&
                    this.ValueType == other.ValueType &&
                    this.StateType == other.StateType &&
                    this.Doubleable == other.Doubleable &&
                    this.Splittable == other.Splittable;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.SumValue, this.ValueType, this.StateType, this.Doubleable, this.Splittable);
        }

        public override string ToString()
        {
            return $"[{this.SumValue}, {this.ValueType}, {this.StateType}, {(this.Doubleable ? "Doubleable" : "Not Doubleable")}, {(this.Splittable ? "Splittable" : "Not Splittable")}]";
        }
    }
}
