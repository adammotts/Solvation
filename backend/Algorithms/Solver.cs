using Solvation.Models;
using Solvation.Enums;
using System.Text;

namespace Solvation.Algorithms
{
    public class Solver
    {
        public static Dictionary<DealerState, Dictionary<DealerState, double>> GenerateDealerTree()
        {
            // Map of all nodes to the probability of attaining each of the various terminal nodes
            var dealerTree = new Dictionary<DealerState, Dictionary<DealerState, double>>();

            // All terminal nodes and the probabilities of attaining them
            var terminalNodeProbabilities = new Dictionary<DealerState, double>();

            foreach (DealerState terminalNode in DealerState.AllTerminalStates())
            {
                terminalNodeProbabilities[terminalNode] = 0.0;
            }

            foreach (DealerState node in DealerState.AllTerminalStates())
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
                    Card[] deck = Card.Deck();

                    // Compute the probability of reaching various terminal nodes
                    foreach (var card in deck)
                    {
                        DealerState resultAfterAddCard = node.Hit(card);

                        // Check if the resulting node is already in the tree
                        if (!dealerTree.TryGetValue(resultAfterAddCard, out var resultNodeProbabilities))
                        {
                            throw new KeyNotFoundException("The resulting dealer state is not present in the dealer tree.");
                        }

                        foreach (var terminalNode in resultNodeProbabilities.Keys)
                        {
                            // Update probabilities for the terminal nodes
                            probabilities[terminalNode] += resultNodeProbabilities[terminalNode] / deck.Count();
                        }
                    }
                }

                // Add the computed probabilities for this node to the dealer tree
                dealerTree[node] = probabilities;
            }

            return dealerTree;
        }

        public static string PrintDealerTree(Dictionary<DealerState, Dictionary<DealerState, double>> dealerTree)
        {
            var result = new StringBuilder();

            foreach (var node in dealerTree.Keys)
            {
                result.AppendLine($"{node} {{");
                foreach (var terminalNode in dealerTree[node].Keys)
                {
                    result.AppendLine($"\t{terminalNode}: {dealerTree[node][terminalNode]}");
                }
                result.AppendLine("}");
            }

            return result.ToString();
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