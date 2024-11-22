using Solvation.Enums;

namespace Solvation.Models
{
    public class DealerState : PileState<DealerState>
    {
        public readonly bool Insurable;

        public DealerState(int sumValue, GameStateValueType valueType) : base(sumValue, valueType)
        {
            this.Insurable = false;
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
                return new DealerState(21, GameStateValueType.Blackjack);
            }

            return notBlackjack;
        }

        public static List<DealerState> AllDealerStates()
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
    }
}
