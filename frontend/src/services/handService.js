import BaseApiService from './baseApiService.js';

class HandService extends BaseApiService {
  async getHand(handId) {
    return this.request(`/hand/${handId}`);
  }

  async getHands() {
    return this.request('/hand');
  }

  async deleteHands() {
    return this.request('/hand', { method: 'DELETE' });
  }
}

export default HandService;
