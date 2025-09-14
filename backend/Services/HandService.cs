using MongoDB.Driver;
using Solvation.Models;
using Solvation.Enums;

namespace Solvation.Services
{
    public interface IHandService
    {
        Task<Hand> GetHandByIdAsync(string id);
        Task<Hand[]> GetHandsAsync();
        // Task DeleteHandsAsync();
        Task UpdateHandAsync(Hand hand);
        Task<Hand> GetHandWithGameStateAsync(string id);
    }

    public class HandService : IHandService
    {
        private readonly IMongoCollection<Hand> _handCollection;
        private readonly IMongoCollection<GameState> _gameStateCollection;
        private readonly IGameService _gameService;

        public HandService(MongoDbService mongoDbService, IGameService gameService)
        {
            _handCollection = mongoDbService.GetCollection<Hand>("hands");
            _gameStateCollection = mongoDbService.GetCollection<GameState>("gameStates");
            _gameService = gameService;
        }

        public async Task<Hand> GetHandByIdAsync(string id)
        {
            var hand = await _handCollection.Find(h => h.Id == id).FirstOrDefaultAsync();

            if (hand == null)
            {
                throw new System.Exception("Hand not found");
            }

            return hand;
        }

        public async Task<Hand[]> GetHandsAsync()
        {
            return (await _handCollection.Find(Builders<Hand>.Filter.Empty).ToListAsync()).ToArray();
        }

        // public async Task DeleteHandsAsync()
        // {
        //     await _handCollection.DeleteManyAsync(Builders<Hand>.Filter.Empty);
        // }

        public async Task UpdateHandAsync(Hand hand)
        {
            var handUpdate = Builders<Hand>.Update.Set(h => h.PlayerCards, hand.PlayerCards);
            await _handCollection.UpdateOneAsync(h => h.Id == hand.Id, handUpdate);
        }

        public async Task<Hand> GetHandWithGameStateAsync(string id)
        {
            var hand = await GetHandByIdAsync(id);

            Card[] playerCards = hand.PlayerCards.ToArray();
            Card dealerCard = hand.DealerCards.First();

            PlayerState playerState = PlayerState.FromCards(playerCards);
            DealerState dealerState = DealerState.FromCard(dealerCard);

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

            return hand;
        }
    }
}
