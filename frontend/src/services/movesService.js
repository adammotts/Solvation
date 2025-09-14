import BaseApiService from './baseApiService.js';

class MovesService extends BaseApiService {
  async getMoves(playerCards, dealerCard) {
    return this.request('/moves', {
      method: 'GET',
      body: JSON.stringify({
        PlayerCards: playerCards,
        DealerCard: dealerCard,
      }),
    });
  }
}

export default MovesService;
