import BaseApiService from './baseApiService.js';

class GameService extends BaseApiService {
  async getGameExpectedValue() {
    return this.request('/game/expected-value');
  }

  async generateGameExpectedValue() {
    return this.request('/game/expected-value', { method: 'POST' });
  }

  async deleteGameExpectedValues() {
    return this.request('/game/expected-values', { method: 'DELETE' });
  }

  async getGameStates() {
    return this.request('/game/states');
  }

  async generateGameStates() {
    return this.request('/game/states', { method: 'POST' });
  }

  async deleteGameStates() {
    return this.request('/game/states', { method: 'DELETE' });
  }

  async resetGameStates() {
    return this.request('/game/states', { method: 'PUT' });
  }
}

export default GameService;
