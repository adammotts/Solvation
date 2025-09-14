import BaseApiService from './baseApiService.js';

class HealthService extends BaseApiService {
    async getHealth() {
        return this.request('/health');
    }
}

export default HealthService;
