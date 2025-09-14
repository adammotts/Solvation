using MongoDB.Driver;
using Solvation.Models;
using Solvation.Enums;

namespace Solvation.Services
{
    public interface ISessionService
    {
        Task<Session> CreateSessionAsync();
        Task<Session> GetSessionByIdAsync(string id);
        // Task DeleteSessionsAsync();
        Task<Hand> GetHandFromSessionAsync(Session session);
        Task UpdateSessionHandIndexAsync(Session session);
        Task UpdateSessionAnalyticsAsync(Session session, Hand hand, string move, string label);
    }

    public class SessionService : ISessionService
    {
        private readonly IMongoCollection<Hand> _handCollection;
        private readonly IMongoCollection<Session> _sessionCollection;
        private readonly IGameService _gameService;

        public SessionService(MongoDbService mongoDbService, IGameService gameService)
        {
            _handCollection = mongoDbService.GetCollection<Hand>("hands");
            _sessionCollection = mongoDbService.GetCollection<Session>("sessions");
            _gameService = gameService;
        }

        public async Task<Session> CreateSessionAsync()
        {
            Hand[] hands = new Hand[10];

            for (int i = 0; i < hands.Length; i++)
            {
                hands[i] = new Hand();
            }

            await _handCollection.InsertManyAsync(hands);

            Session session = new Session(hands);
            await _sessionCollection.InsertOneAsync(session);

            return session;
        }

        public async Task<Session> GetSessionByIdAsync(string id)
        {
            var session = await _sessionCollection.Find(s => s.Id == id).FirstOrDefaultAsync();

            if (session == null)
            {
                throw new System.Exception("Session not found");
            }

            return session;
        }

        // public async Task DeleteSessionsAsync()
        // {
        //     await _sessionCollection.DeleteManyAsync(Builders<Session>.Filter.Empty);
        // }

        public async Task<Hand> GetHandFromSessionAsync(Session session)
        {
            var currentHandId = session.CurrentHandId();

            var hand = await _handCollection.Find(h => h.Id == currentHandId).FirstOrDefaultAsync();

            if (hand == null)
            {
                throw new System.Exception("Hand not found");
            }

            return hand;
        }

        public async Task UpdateSessionHandIndexAsync(Session session)
        {
            var sessionUpdate = Builders<Session>.Update.Set(s => s.CurrentHandIndex, session.CurrentHandIndex);
            await _sessionCollection.UpdateOneAsync(s => s.Id == session.Id, sessionUpdate);
        }

        public async Task UpdateSessionAnalyticsAsync(Session session, Hand hand, string move, string label)
        {
            Actions actions = await _gameService.GetActionsFromStatesAsync(hand.CurrentPlayerState(), hand.CurrentDealerState());

            double evBestMove = actions.BestMoveEV();
            double evMove = actions.MoveEV(move);

            var sessionUpdate = Builders<Session>.Update
                .Set(s => s.ExpectedValueLoss, session.ExpectedValueLoss + evBestMove - evMove)
                .Set(s => s.Statistics, session.Statistics.Update(label));
            await _sessionCollection.UpdateOneAsync(s => s.Id == session.Id, sessionUpdate);
        }
    }
}
