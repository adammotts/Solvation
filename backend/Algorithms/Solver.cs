using Solvation.Models;
using Solvation.Enums;
using System.Text;

namespace Solvation.Algorithms
{
    public class Solver
    {
        /*
            Pre Reveal: Tree before player and dealer blackjack reveal (dealer has a hidden card, no player actions can be taken)
            Post Reveal: Tree after player and dealer blackjack reveal (dealer has a hidden card, but blackjack is not possible)
        */

        public readonly static Dictionary<DealerState, Dictionary<DealerState, double>> DealerPostRevealTree = GenerateDealerPostRevealTree();

        public readonly static Dictionary<DealerState, Dictionary<DealerState, double>> DealerBlackjackTree = GenerateDealerBlackjackTree();

        public readonly static Dictionary<DealerState, Dictionary<PlayerState, Actions>> PlayerPostRevealTree = GeneratePlayerPostRevealTree();

        private static Dictionary<DealerState, Dictionary<DealerState, double>> GenerateDealerPostRevealTree()
        {
            // Map of all nodes to the probability of attaining each of the various terminal nodes
            var dealerTree = new Dictionary<DealerState, Dictionary<DealerState, double>>();

            // All terminal nodes and the probabilities of attaining them
            var terminalNodeProbabilities = new Dictionary<DealerState, double>();
            
            Card[] allRanks = Card.AllRanks();

            foreach (DealerState terminalNode in DealerState.AllTerminalStates())
            {
                if (terminalNode.ValueType == GameStateValueType.Blackjack)
                {
                    continue;
                }

                terminalNodeProbabilities[terminalNode] = 0.0;
            }

            foreach (DealerState node in DealerState.AllStates())
            {
                if (node.ValueType == GameStateValueType.Blackjack)
                {
                    continue;
                }

                // Initialize probabilities with all terminal nodes having 0 probability
                var probabilities = new Dictionary<DealerState, double>(terminalNodeProbabilities);

                // All terminal nodes have a 100% chance of reaching a terminal node (themselves)
                if (node.StateType == GameStateType.Terminal)
                {
                    probabilities[node] = 1.0;
                }
                else
                {
                    // Given that we are in post reveal, we know that the hidden card is not a 10
                    if (node.SumValue == 11 && node.ValueType == GameStateValueType.Soft && node.Insurable)
                    {
                        Card[] nonTenRanks = allRanks.Where(card => DealerState.RankValues[card.Rank].SumValue != 10).ToArray();

                        Solver.DealerAddEachCard(nonTenRanks, node, probabilities, dealerTree);
                    }

                    // Given that we are in post reveal, we know that the hidden card is not an ace
                    else if (node.SumValue == 10 && node.ValueType == GameStateValueType.Hard && node.Insurable)
                    {
                        Card[] nonAceRanks = allRanks.Where(card => DealerState.RankValues[card.Rank].SumValue != 11).ToArray();

                        Solver.DealerAddEachCard(nonAceRanks, node, probabilities, dealerTree);
                    }

                    else
                    {
                        Solver.DealerAddEachCard(allRanks, node, probabilities, dealerTree);
                    }
                }

                // Add the computed probabilities for this node to the dealer tree
                dealerTree[node] = probabilities;
            }

            return dealerTree;
        }

        private static void DealerAddEachCard(Card[] cards, DealerState node, Dictionary<DealerState, double> probabilities, Dictionary<DealerState, Dictionary<DealerState, double>> dealerTree)
        {
            foreach (var card in cards)
            {
                DealerState resultAfterAddCard = node.Hit(card);

                if (!dealerTree.TryGetValue(resultAfterAddCard, out var resultNodeProbabilities))
                {
                    throw new KeyNotFoundException($"{node} + {card} = {resultAfterAddCard} not found in dealer tree");
                }

                foreach (var terminalNode in resultNodeProbabilities.Keys)
                {
                    probabilities[terminalNode] += resultNodeProbabilities[terminalNode] / cards.Count();
                }
            }
        }

        private static Dictionary<DealerState, Dictionary<DealerState, double>> GenerateDealerBlackjackTree()
        {
            var dealerBlackjackTree = new Dictionary<DealerState, Dictionary<DealerState, double>>();

            var blackjackProbability = new Dictionary<DealerState, double>();

            DealerState blackjack = new DealerState(21, GameStateValueType.Blackjack);

            blackjackProbability[blackjack] = 0.0;

            foreach (DealerState node in DealerState.AllStates())
            {
                var probabilities = new Dictionary<DealerState, double>(blackjackProbability);

                if (node.ValueType == GameStateValueType.Blackjack)
                {
                    probabilities[blackjack] = 1.0;
                }
                else if (node.StateType == GameStateType.Terminal)
                {
                    probabilities[blackjack] = 0;
                }
                else
                {
                    Card[] allRanks = Card.AllRanks();

                    foreach (var card in allRanks)
                    {
                        DealerState resultAfterAddCard = node.Reveal(card);

                        if (!dealerBlackjackTree.TryGetValue(resultAfterAddCard, out var resultNodeProbabilities))
                        {
                            throw new KeyNotFoundException($"{node} + {card} = {resultAfterAddCard} not found in dealer blackjack tree");
                        }

                        probabilities[blackjack] += resultNodeProbabilities[blackjack] / allRanks.Count();
                    }
                }

                if (probabilities.Count != 1)
                {
                    throw new InvalidOperationException("Dealer blackjack tree should not have entries other than blackjack");
                }

                dealerBlackjackTree[node] = probabilities;
            }

            return dealerBlackjackTree;
        }

        private static string ViewDealerTree(Dictionary<DealerState, Dictionary<DealerState, double>> dealerTree)
        {
            StringBuilder result = new StringBuilder();
            foreach (DealerState node in dealerTree.Keys)
            {
                result.AppendLine($"{node} {{");
                foreach (DealerState terminalNode in dealerTree[node].Keys)
                {
                    result.AppendLine($"\t{terminalNode}: {dealerTree[node][terminalNode]}");
                }
                result.AppendLine("}");
            }

            return result.ToString();
        }

        private static Dictionary<DealerState, Dictionary<PlayerState, Actions>> GeneratePlayerPostRevealTree()
        {
            // The dealer's tree
            var dealerTree = Solver.DealerPostRevealTree;

            var strategyTree = new Dictionary<DealerState, Dictionary<PlayerState, Actions>>();

            foreach (DealerState dealerNode in dealerTree.Keys)
            {
                if (dealerNode.StateType == GameStateType.Terminal)
                {
                    continue;
                }

                var playerStrategyGivenDealerState = GeneratePlayerStrategyTreeGivenDealerState(dealerNode);

                strategyTree[dealerNode] = playerStrategyGivenDealerState;
            }

            return strategyTree;
        }

        private static Dictionary<PlayerState, Actions> GeneratePlayerStrategyTreeGivenDealerState(DealerState dealerNode)
        {
            var playerStrategyGivenDealerState = new Dictionary<PlayerState, Actions>();

            PlayerState[] allStates = PlayerState.AllStates().Where(node => node.ValueType != GameStateValueType.Blackjack).ToArray();

            Card[] allRanks = Card.AllRanks();

            foreach (PlayerState playerNode in allStates)
            {
                Actions expectedValues = new Actions(null, null, null, null);

                double evStand = StandExpectedValue(playerNode, dealerNode);

                if (playerNode.StateType == GameStateType.Terminal)
                {
                    expectedValues.Hit = evStand;
                    expectedValues.Stand = evStand;
                    expectedValues.Double = evStand;
                    expectedValues.Split = evStand;
                }

                // Not doubleable or splittable
                else if (!playerNode.Doubleable)
                {
                    double evHit = 0;

                    foreach (var card in allRanks)
                    {
                        PlayerMovesAfterAddCard(playerNode, card, playerStrategyGivenDealerState, out double evResultHit, out double evResultStand);

                        evHit += BestMove(evResultHit, evResultStand) / allRanks.Count();
                    }

                    expectedValues.Hit = evHit;
                    expectedValues.Stand = evStand;
                }

                // Doubleable, not splittable
                else if (!playerNode.Splittable)
                {
                    double evHit = 0;
                    double evDouble = 0;

                    foreach (var card in allRanks)
                    {
                        PlayerMovesAfterAddCard(playerNode, card, playerStrategyGivenDealerState, out double evResultHit, out double evResultStand);

                        evHit += BestMove(evResultHit, evResultStand) / allRanks.Count();

                        evDouble += 2 * evResultStand / allRanks.Count();
                    }

                    expectedValues.Hit = evHit;
                    expectedValues.Stand = evStand;
                    expectedValues.Double = evDouble;
                }

                // All splittable states are also doubleable
                else
                {
                    double evSplit = 0;

                    foreach (var card in allRanks)
                    {
                        PlayerMovesAfterSplit(playerNode, card, playerStrategyGivenDealerState, out double evResultHit, out double evResultStand, out double evResultDouble);

                        evSplit += 2 * BestMove(evResultHit, evResultStand, evResultDouble) / allRanks.Count();
                    }

                    Actions declineSplitActions = playerStrategyGivenDealerState[playerNode.DeclineSplit()];
                    expectedValues.Hit = declineSplitActions.Hit;
                    expectedValues.Stand = declineSplitActions.Stand;
                    expectedValues.Double = declineSplitActions.Double;
                    expectedValues.Split = evSplit;
                }

                playerStrategyGivenDealerState[playerNode] = expectedValues;
            }

            return playerStrategyGivenDealerState;
        }

        private static void PlayerMovesAfterAddCard(PlayerState playerNode, Card card, Dictionary<PlayerState, Actions> playerStrategyGivenDealerState, out double evResultHit, out double evResultStand)
        {
            PlayerState resultAfterAddCard = playerNode.Hit(card);

            if (!playerStrategyGivenDealerState.TryGetValue(resultAfterAddCard, out var resultNodeActions))
            {
                throw new KeyNotFoundException($"{playerNode} + {card} = {resultAfterAddCard} not found in player tree");
            }

            if (resultNodeActions.Hit.HasValue)
            {
                evResultHit = resultNodeActions.Hit.Value;
            }
            else
            {
                throw new InvalidOperationException("Hit expected value should not be null");
            }

            if (resultNodeActions.Stand.HasValue)
            {
                evResultStand = resultNodeActions.Stand.Value;
            }
            else
            {
                throw new InvalidOperationException("Stand expected value should not be null");
            }
        }

        private static void PlayerMovesAfterSplit(PlayerState playerNode, Card card, Dictionary<PlayerState, Actions> playerStrategyGivenDealerState, out double evResultHit, out double evResultStand, out double evResultDouble)
        {
            PlayerState resultAfterAddCard = playerNode.Split(card);

            if (!playerStrategyGivenDealerState.TryGetValue(resultAfterAddCard, out var resultNodeActions))
            {
                throw new KeyNotFoundException($"Split {playerNode} + {card} = {resultAfterAddCard} not found in player tree");
            }

            if (resultNodeActions.Hit.HasValue)
            {
                evResultHit = resultNodeActions.Hit.Value;
            }
            else
            {
                throw new InvalidOperationException("Hit expected value should not be null");
            }

            if (resultNodeActions.Stand.HasValue)
            {
                evResultStand = resultNodeActions.Stand.Value;
            }
            else
            {
                throw new InvalidOperationException("Stand expected value should not be null");
            }

            if (resultNodeActions.Double.HasValue)
            {
                evResultDouble = resultNodeActions.Double.Value;
            }
            else
            {
                throw new InvalidOperationException("Double expected value should not be null");
            }
        }

        private static double BestMove(params double[] evs)
        {
            return evs.Max();
        }

        private static double StandExpectedValue(PlayerState playerNode, DealerState dealerNode)
        {
            if (playerNode.ValueType == GameStateValueType.Blackjack || dealerNode.ValueType == GameStateValueType.Blackjack)
            {
                throw new InvalidOperationException("Blackjack should not be in stand expected value");
            }

            var dealerTree = Solver.DealerPostRevealTree;

            double ev = 0;

            var dealerTreeCard = dealerTree[dealerNode];

            foreach (DealerState terminalNode in dealerTreeCard.Keys)
            {
                double frequency = dealerTreeCard[terminalNode];

                // Player bust
                if (playerNode.SumValue > 21)
                {
                    ev -= 1 * frequency;
                }
                // Dealer bust
                else if (terminalNode.SumValue > 21)
                {
                    ev += 1 * frequency;
                }
                // Dealer win
                else if (terminalNode.SumValue > playerNode.SumValue)
                {
                    ev -= 1 * frequency;
                }
                // Player win
                else if (terminalNode.SumValue < playerNode.SumValue)
                {
                    ev += 1 * frequency;
                }
                // Tie
                else
                {
                    ev += 0;
                }
            }

            return ev;
        }

        public static string ViewPlayerTree(Dictionary<DealerState, Dictionary<PlayerState, Actions>> playerTree)
        {
            StringBuilder result = new StringBuilder();
            foreach (DealerState dealerNode in playerTree.Keys)
            {
                result.AppendLine($"[Dealer] {dealerNode} {{");
                foreach (PlayerState playerNode in playerTree[dealerNode].Keys)
                {
                    result.AppendLine($"\t[Player] {playerNode}: {playerTree[dealerNode][playerNode]}");
                }
                result.AppendLine("}");
            }

            return result.ToString();
        }

        private static bool DealerInteractions()
        {
            string revealInteractions = DealerState.RevealInteractions();
            string interactions = DealerState.Interactions();
            string dealerBlackjackTree = Solver.ViewDealerTree(Solver.DealerBlackjackTree);
            string dealerTree = Solver.ViewDealerTree(Solver.DealerPostRevealTree);
            string result = $"Dealer Reveal Interactions:\n\n{revealInteractions}\nDealer Hit Interactions:\n\n{interactions}\nDealer Blackjack Tree:\n\n{dealerBlackjackTree}\nDealer Post Reveal Tree:\n\n{dealerTree}";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Test/DealerInteractions.txt");
            string existingInteractions = File.ReadAllText(filePath);
            File.WriteAllText(filePath, result);
            return existingInteractions == "" || existingInteractions == result;
        }

        private static bool PlayerInteractions()
        {
            string interactions = PlayerState.Interactions();
            string splitInteractions = PlayerState.SplitInteractions();
            string playerTree = Solver.ViewPlayerTree(Solver.PlayerPostRevealTree);
            string result = $"Player Hit Interactions:\n\n{interactions}\nPlayer Split Interactions:\n\n{splitInteractions}\nPlayer Tree:\n\n{playerTree}";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Test/PlayerInteractions.txt");
            string existingInteractions = File.ReadAllText(filePath);
            File.WriteAllText(filePath, result);
            return existingInteractions == "" || existingInteractions == result;
        }

        public static void VerifyInteractions()
        {
            bool dealerVerified = Solver.DealerInteractions();
            bool playerVerified = Solver.PlayerInteractions();
            StringBuilder result = new StringBuilder();

            if (!dealerVerified)
            {
                result.AppendLine("Dealer interactions verification failed");
            }

            if (!playerVerified)
            {
                result.AppendLine("Player interactions verification failed");
            }

            if (result.Length != 0)
            {
                throw new InvalidOperationException(result.ToString());
            }
        }

        public static GameState[] Solve()
        {
            List<GameState> gameStates = new List<GameState>();

            var playerStrategyTree = Solver.PlayerPostRevealTree;

            foreach (DealerState dealerState in playerStrategyTree.Keys)
            {
                var playerStrategyGivenDealerState = playerStrategyTree[dealerState];

                foreach (PlayerState playerState in playerStrategyGivenDealerState.Keys)
                {
                    Actions expectedValues = playerStrategyGivenDealerState[playerState];

                    gameStates.Add(
                        new GameState(
                            playerState,
                            dealerState,
                            expectedValues
                        )
                    );
                }
            }

            return gameStates.ToArray();
        }

        public static double GameExpectedValue(int epochs = 1000000)
        {
            double ev = 0;

            for (int i = 0; i < epochs; i++)
            {
                Hand hand = new Hand(true);
                
                PlayerState playerState = PlayerState.FromCards(hand.PlayerCards.ToArray(), allowBlackjack: true);

                // Currently I have no functionality to construct a post reveal state for the dealer, so a player state to simulate blackjack is used
                PlayerState dealerRevealState = PlayerState.FromCards(hand.DealerCards.ToArray(), allowBlackjack: true);

                // Console.WriteLine($"Player: {hand.PlayerCards.First()}, {hand.PlayerCards.Last()} | {playerState}, Dealer: {hand.DealerCards.First()}, {hand.DealerCards.Last()} | {dealerRevealState} | {hand.CurrentDealerState()}");

                if (playerState.ValueType == GameStateValueType.Blackjack)
                {
                    
                    ev += 1.5;
                }

                else if (dealerRevealState.ValueType == GameStateValueType.Blackjack)
                {
                    ev -= 1;
                }

                else {
                    DealerState dealerPreRevealState = hand.CurrentDealerState();

                    Actions expectedValues = Solver.PlayerPostRevealTree[dealerPreRevealState][playerState];

                    ev += expectedValues.BestMoveEV();
                }
            }

            return ev / epochs;
        }
    }
}