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
                    Card[] allRanks = Card.AllRanks();

                    // Given that we are in post reveal, we know that the hidden card is not a 10
                    if (node.SumValue == 11 && node.ValueType == GameStateValueType.Soft && node.Insurable)
                    {
                        Card[] nonTenRanks = allRanks.Where(card => DealerState.RankValues[card.Rank].SumValue != 10).ToArray();

                        foreach (var card in nonTenRanks)
                        {
                            DealerState resultAfterAddCard = node.Hit(card);

                            if (!dealerTree.TryGetValue(resultAfterAddCard, out var resultNodeProbabilities))
                            {
                                throw new KeyNotFoundException($"{node} + {card} = {resultAfterAddCard} not found in dealer tree");
                            }

                            foreach (var terminalNode in resultNodeProbabilities.Keys)
                            {
                                probabilities[terminalNode] += resultNodeProbabilities[terminalNode] / nonTenRanks.Count();
                            }
                        }
                    }

                    // Given that we are in post reveal, we know that the hidden card is not an ace
                    else if (node.SumValue == 10 && node.ValueType == GameStateValueType.Hard && node.Insurable)
                    {
                        Card[] nonAceRanks = allRanks.Where(card => DealerState.RankValues[card.Rank].SumValue != 11).ToArray();

                        foreach (var card in nonAceRanks)
                        {
                            DealerState resultAfterAddCard = node.Hit(card);

                            if (!dealerTree.TryGetValue(resultAfterAddCard, out var resultNodeProbabilities))
                            {
                                throw new KeyNotFoundException($"{node} + {card} = {resultAfterAddCard} not found in dealer tree");
                            }

                            foreach (var terminalNode in resultNodeProbabilities.Keys)
                            {
                                probabilities[terminalNode] += resultNodeProbabilities[terminalNode] / nonAceRanks.Count();
                            }
                        }
                    }

                    else
                    {
                        // Compute the probability of reaching various terminal nodes
                        foreach (var card in allRanks)
                        {
                            DealerState resultAfterAddCard = node.Hit(card);

                            if (!dealerTree.TryGetValue(resultAfterAddCard, out var resultNodeProbabilities))
                            {
                                throw new KeyNotFoundException($"{node} + {card} = {resultAfterAddCard} not found in dealer tree");
                            }

                            foreach (var terminalNode in resultNodeProbabilities.Keys)
                            {
                                probabilities[terminalNode] += resultNodeProbabilities[terminalNode] / allRanks.Count();
                            }
                        }
                    }
                }

                // Add the computed probabilities for this node to the dealer tree
                dealerTree[node] = probabilities;
            }

            return dealerTree;
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

                        foreach (var terminalNode in resultNodeProbabilities.Keys)
                        {
                            probabilities[blackjack] += resultNodeProbabilities[terminalNode] / allRanks.Count();
                        }
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

            foreach (PlayerState playerNode in allStates)
            {
                Actions expectedValues = new Actions(0, 0, 0, 0);

                if (playerNode.StateType == GameStateType.Terminal)
                {
                    double ev = StandExpectedValue(playerNode, dealerNode);

                    expectedValues.Hit = ev;
                    expectedValues.Stand = ev;
                    expectedValues.Double = ev;
                    expectedValues.Split = ev;
                }

                else if (!playerNode.Splittable)
                {
                    Card[] allRanks = Card.AllRanks();

                    expectedValues.Stand = StandExpectedValue(playerNode, dealerNode);

                    foreach (var card in allRanks)
                    {
                        PlayerState resultAfterAddCard = playerNode.Hit(card);

                        if (!playerStrategyGivenDealerState.TryGetValue(resultAfterAddCard, out var resultNodeActions))
                        {
                            throw new KeyNotFoundException($"{playerNode} + {card} = {resultAfterAddCard} not found in player tree");
                        }

                        double evBestMove = double.MinValue;

                        if (resultNodeActions.Hit > evBestMove)
                        {
                            evBestMove = resultNodeActions.Hit;
                        }
                        if (resultNodeActions.Stand > evBestMove)
                        {
                            evBestMove = resultNodeActions.Stand;
                        }

                        expectedValues.Hit += evBestMove / allRanks.Count();

                        expectedValues.Double += 2 * resultNodeActions.Stand / allRanks.Count();
                    }
                }

                else
                {
                    Card[] allRanks = Card.AllRanks();

                    double evSplit = 0;

                    PlayerState splitNode = playerNode.Split();

                    foreach (var card in allRanks)
                    {
                        PlayerState resultAfterAddCard = splitNode.Hit(card);

                        if (!playerStrategyGivenDealerState.TryGetValue(resultAfterAddCard, out var resultNodeActions))
                        {
                            throw new KeyNotFoundException($"{splitNode} + {card} = {resultAfterAddCard} not found in player tree");
                        }

                        double evBestMove = double.MinValue;

                        if (resultNodeActions.Hit > evBestMove)
                        {
                            evBestMove = resultNodeActions.Hit;
                        }
                        if (resultNodeActions.Stand > evBestMove)
                        {
                            evBestMove = resultNodeActions.Stand;
                        }
                        if (resultNodeActions.Double > evBestMove)
                        {
                            evBestMove = resultNodeActions.Double;
                        }

                        evSplit += 2 * evBestMove / allRanks.Count();
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
            string playerTree = Solver.ViewPlayerTree(Solver.PlayerPostRevealTree);
            string result = $"Player Interactions:\n\n{interactions}\nPlayer Tree:\n\n{playerTree}";
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
                    var expectedValues = playerStrategyGivenDealerState[playerState];

                    gameStates.Add(
                        new GameState(
                            playerState.SumValue,
                            playerState.ValueType,
                            playerState.StateType,
                            dealerState.SumValue,
                            dealerState.ValueType,
                            dealerState.StateType,
                            expectedValues
                        )
                    );
                }
            }

            return gameStates.ToArray();
        }
    }
}