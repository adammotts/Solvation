import BaseApiService from './baseApiService.js';

class SessionService extends BaseApiService {
  async createSession() {
    return this.request('/session', { method: 'POST' });
  }

  async getSession(sessionId) {
    return this.request(`/session/${sessionId}`);
  }

  async makeMove(sessionId, move, label) {
    return this.request(`/session/${sessionId}`, {
      method: 'PATCH',
      body: JSON.stringify({ move, label }),
    });
  }

  async deleteSessions() {
    return this.request('/session', { method: 'DELETE' });
  }
}

export default SessionService;
