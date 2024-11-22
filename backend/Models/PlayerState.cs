using Solvation.Enums;

namespace Solvation.Models
{
    public class PlayerState : PileState<PlayerState>
    {
        public readonly bool Splittable;

        public PlayerState(int sumValue, GameStateValueType valueType) : base(sumValue, valueType)
        {
            this.Splittable = false;
        }

        public PlayerState(int sumValue, GameStateValueType valueType, bool splittable) : base(sumValue, valueType)
        {
            this.Splittable = splittable;
        }

        protected override GameStateType DetermineStateType()
        {
            return this.SumValue >= 21 ? GameStateType.Terminal : GameStateType.Active;
        }

        public override PlayerState Hit(Card card)
        {
            return this.AddCard(card);
        }

        public PlayerState Split()
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

            return new PlayerState(splitValue, splitValueType);
        }

        public new static List<PlayerState> AllStates()
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
