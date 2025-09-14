using MongoDB.Driver;
using Solvation.Models;
using Solvation.Algorithms;

namespace Solvation.Services
{
    public interface IGameService
    {
        Task<double> GenerateGameExpectedValueAsync();
        // Task DeleteGameExpectedValuesAsync();
        Task<GameActions> GetGameExpectedValueAsync();
        Task<GameState[]> GenerateGameStatesAsync();
        // Task DeleteGameStatesAsync();
        Task<GameState[]> GetGameStatesAsync();
        Task<GameState[]> ResetGameStatesAsync();
        Task<Actions> GetActionsFromStatesAsync(PlayerState playerState, DealerState dealerState);
    }

    public class GameService : IGameService
    {
        private readonly IMongoCollection<GameActions> _gameExpectedValueCollection;
        private readonly IMongoCollection<GameState> _gameStateCollection;

        public GameService(MongoDbService mongoDbService)
        {
            _gameExpectedValueCollection = mongoDbService.GetCollection<GameActions>("gameExpectedValues");
            _gameStateCollection = mongoDbService.GetCollection<GameState>("gameStates");
        }

        public async Task<double> GenerateGameExpectedValueAsync()
        {
            double gameExpectedValue = Solver.GameExpectedValue();
            await _gameExpectedValueCollection.InsertOneAsync(new GameActions(gameExpectedValue));
            return gameExpectedValue;
        }

        // public async Task DeleteGameExpectedValuesAsync()
        // {
        //     await _gameExpectedValueCollection.DeleteManyAsync(Builders<GameActions>.Filter.Empty);
        // }

        public async Task<GameActions> GetGameExpectedValueAsync()
        {
            var gameExpectedValues = await _gameExpectedValueCollection.Find(Builders<GameActions>.Filter.Empty).ToListAsync();

            if (!gameExpectedValues.Any())
            {
                throw new System.Exception("No Game Expected Value found.");
            }

            if (gameExpectedValues.Count > 1)
            {
                throw new System.Exception("Multiple GameStates found. " + gameExpectedValues.Count);
            }

            return gameExpectedValues.First();
        }

        public async Task<GameState[]> GenerateGameStatesAsync()
        {
            GameState[] gameStates = Solver.Solve();
            await _gameStateCollection.InsertManyAsync(gameStates);
            return gameStates;
        }

        // public async Task DeleteGameStatesAsync()
        // {
        //     await _gameStateCollection.DeleteManyAsync(Builders<GameState>.Filter.Empty);
        // }

        public async Task<GameState[]> GetGameStatesAsync()
        {
            return (await _gameStateCollection.Find(Builders<GameState>.Filter.Empty).ToListAsync()).ToArray();
        }

        public async Task<GameState[]> ResetGameStatesAsync()
        {
            await _gameStateCollection.DeleteManyAsync(Builders<GameState>.Filter.Empty);
            GameState[] gameStates = Solver.Solve();
            await _gameStateCollection.InsertManyAsync(gameStates);
            return gameStates;
        }

        public async Task<Actions> GetActionsFromStatesAsync(PlayerState playerState, DealerState dealerState)
        {
            var filter = Builders<GameState>.Filter.And(
                Builders<GameState>.Filter.Eq(g => g.PlayerState, playerState),
                Builders<GameState>.Filter.Eq(g => g.DealerState, dealerState)
            );

            var gameStates = await _gameStateCollection.Find(filter).ToListAsync();

            if (!gameStates.Any())
            {
                throw new System.Exception("No matching GameStates found.");
            }

            if (gameStates.Count > 1)
            {
                throw new System.Exception("Multiple GameStates found. " + gameStates.Count);
            }

            return gameStates.First().Actions;
        }
    }
}
