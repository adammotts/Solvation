const API_BASE_URL = process.env.REACT_APP_API_BASE_URL;

class ApiService {
    constructor() {
        this.baseURL = API_BASE_URL;
    }

    async request(endpoint, options = {}) {
        const url = `${this.baseURL}${endpoint}`;
        const config = {
            headers: {
                'Content-Type': 'application/json',
                ...options.headers,
            },
            ...options,
        };

        try {
            const response = await fetch(url, config);

            if (!response.ok) {
                const errorData = await response.json().catch(() => ({}));
                throw new Error(errorData.message || `HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('API request failed:', error);
            throw error;
        }
    }

    // Health endpoints
    async getHealth() {
        return this.request('/health');
    }

    // Game endpoints
    async getGameExpectedValue() {
        return this.request('/game/expected-value');
    }

    async generateGameExpectedValue() {
        return this.request('/game/expected-value', { method: 'POST' });
    }

    // async deleteGameExpectedValues() {
    //     return this.request('/game/expected-values', { method: 'DELETE' });
    // }

    async getGameStates() {
        return this.request('/game/states');
    }

    async generateGameStates() {
        return this.request('/game/states', { method: 'POST' });
    }

    // async deleteGameStates() {
    //     return this.request('/game/states', { method: 'DELETE' });
    // }

    async resetGameStates() {
        return this.request('/game/states', { method: 'PUT' });
    }

    // Session endpoints
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

    // async deleteSessions() {
    //     return this.request('/session', { method: 'DELETE' });
    // }

    // Hand endpoints
    async getHand(handId) {
        return this.request(`/hand/${handId}`);
    }

    async getHands() {
        return this.request('/hand');
    }

    // async deleteHands() {
    //     return this.request('/hand', { method: 'DELETE' });
    // }

    // Moves endpoints
    async getMoves(playerCards, dealerCard) {
        return this.request('/moves', {
            method: 'GET',
            body: JSON.stringify({
                PlayerCards: playerCards,
                DealerCard: dealerCard
            }),
        });
    }
}

// Create and export a singleton instance
const apiService = new ApiService();
export default apiService;
