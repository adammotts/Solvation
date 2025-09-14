import HealthService from './healthService.js';
import GameService from './gameService.js';
import SessionService from './sessionService.js';
import HandService from './handService.js';
import MovesService from './movesService.js';

export { default as HealthService } from './healthService.js';
export { default as GameService } from './gameService.js';
export { default as SessionService } from './sessionService.js';
export { default as HandService } from './handService.js';
export { default as MovesService } from './movesService.js';

export const healthService = new HealthService();
export const gameService = new GameService();
export const sessionService = new SessionService();
export const handService = new HandService();
export const movesService = new MovesService();
