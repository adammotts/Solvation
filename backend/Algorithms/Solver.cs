using Solvation.Models;
using Solvation.Enums;
using System.Text;

namespace Solvation.Algorithms
{
    public class Solver
    {
        public readonly static Dictionary<DealerState, Dictionary<DealerState, double>> DealerTree = GenerateDealerTree();

        public readonly static Dictionary<DealerState, Dictionary<PlayerState, Actions>> PlayerTree = GeneratePlayerTree();

        private static Dictionary<DealerState, Dictionary<DealerState, double>> GenerateDealerTree()
        {
            // Map of all nodes to the probability of attaining each of the various terminal nodes
            var dealerTree = new Dictionary<DealerState, Dictionary<DealerState, double>>();

            // All terminal nodes and the probabilities of attaining them
            var terminalNodeProbabilities = new Dictionary<DealerState, double>();

            foreach (DealerState terminalNode in DealerState.AllTerminalStates())
            {
                terminalNodeProbabilities[terminalNode] = 0.0;
            }

            foreach (DealerState node in DealerState.AllStates())
            {
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

                // Add the computed probabilities for this node to the dealer tree
                dealerTree[node] = probabilities;
            }

            return dealerTree;
        }

        public static string ViewDealerTree(Dictionary<DealerState, Dictionary<DealerState, double>> dealerTree)
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

        private static Dictionary<DealerState, Dictionary<PlayerState, Actions>> GeneratePlayerTree()
        {
            // The dealer's tree
            var dealerTree = Solver.DealerTree;

            var strategyTree = new Dictionary<DealerState, Dictionary<PlayerState, Actions>>();

            var playerTree = new Dictionary<PlayerState, Actions>();

            foreach (DealerState dealerNode in dealerTree.Keys)
            {
                var singlePlayerTree = new Dictionary<PlayerState, Actions>(playerTree);

                foreach (PlayerState playerNode in PlayerState.AllStates())
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

                            if (!singlePlayerTree.TryGetValue(resultAfterAddCard, out var resultNodeActions))
                            {
                                throw new KeyNotFoundException($"{dealerNode} + {card} = {resultAfterAddCard} not found in player tree");
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

                            if (!singlePlayerTree.TryGetValue(resultAfterAddCard, out var resultNodeActions))
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

                        Actions declineSplitActions = singlePlayerTree[playerNode.DeclineSplit()];
                        expectedValues.Hit = declineSplitActions.Hit;
                        expectedValues.Stand = declineSplitActions.Stand;
                        expectedValues.Double = declineSplitActions.Double;
                        expectedValues.Split = evSplit;
                    }

                    singlePlayerTree[playerNode] = expectedValues;
                }

                strategyTree[dealerNode] = singlePlayerTree;
            }

            return strategyTree;
        }
        private static double StandExpectedValue(PlayerState playerNode, DealerState dealerNode)
        {
            var dealerTree = Solver.DealerTree;

            double ev = 0;

            var dealerTreeCard = dealerTree[dealerNode];

            foreach (DealerState terminalNode in dealerTreeCard.Keys)
            {
                double frequency = dealerTreeCard[terminalNode];

                // Blackjack tie
                if (terminalNode.ValueType == GameStateValueType.Blackjack && playerNode.ValueType == GameStateValueType.Blackjack)
                {
                    ev += 0;
                }
                // Dealer blackjack
                else if (terminalNode.ValueType == GameStateValueType.Blackjack)
                {
                    ev -= 1 * frequency;
                }
                // Player blackjack
                else if (playerNode.ValueType == GameStateValueType.Blackjack)
                {
                    ev += 1.5 * frequency;
                }
                // Player bust
                else if (playerNode.SumValue > 21)
                {
                    ev -= 1 * frequency;
                }
                // Dealer bust
                else if (dealerNode.SumValue > 21)
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
                result.AppendLine($"{dealerNode} {{");
                foreach (PlayerState playerNode in playerTree[dealerNode].Keys)
                {
                    result.AppendLine($"\t{playerNode}: {playerTree[dealerNode][playerNode]}");
                }
                result.AppendLine("}");
            }

            return result.ToString();
        }

        private static bool DealerInteractions()
        {
            string interactions = DealerState.Interactions();
            string dealerTree = Solver.ViewDealerTree(Solver.DealerTree);
            string result = $"Dealer Interactions:\n\n{interactions}\nDealer Tree:\n\n{dealerTree}";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Test/DealerInteractions.txt");
            string existingInteractions = File.ReadAllText(filePath);
            File.WriteAllText(filePath, result);
            return existingInteractions == "" || existingInteractions == result;
        }

        private static bool PlayerInteractions()
        {
            string interactions = PlayerState.Interactions();
            string playerTree = Solver.ViewPlayerTree(Solver.PlayerTree);
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

            foreach (PlayerState playerState in PlayerState.AllStates())
            {
                foreach (DealerState dealerState in DealerState.AllStates())
                {
                    gameStates.Add(new GameState(
                        playerState.SumValue,
                        playerState.ValueType,
                        playerState.StateType,
                        dealerState.SumValue,
                        dealerState.ValueType,
                        dealerState.StateType
                    ));
                }
            }

            return gameStates.ToArray();
        }
    }
}