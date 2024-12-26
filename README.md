# Solvation
Train your blackjack skills by playing against the dealer and learning the expected values of each move you make. Solved with closed form Dynamic Programming. Solver trained using Q Learning. Designed using the Curiously Recurring Template Pattern (CRTP), also known as F-bound Polymorphism. See video below for walk through.

https://github.com/user-attachments/assets/4dbb4094-30be-4944-92e9-26d41d00f018

## Rules
- Can only split once
- Player blackjack pushes against dealer blackjack
- Dealer stands on soft 17
- No insurance (coming soon)
- You can only get blackjack on initial deal, not splits
- Player can double after split
- Player cannot act after reaching 21, whether soft or hard

## Developer Notes

### Backend
- Run `dotnet run` in `/backend` to start the backend at `localhost:5256`
- If a change was made that affects the logic of player/dealer interactions or probabilities, the command will fail, and you can view changes in `/Test/PlayerInteractions.txt` or `/Test/DealerInteractions.txt`

### Frontend
- Run `npm start` in `/frontend` to start the frontend at `localhost:3000`
- Install linting using `npm install eslint --save-dev` and `npm install --save-dev prettier eslint-config-prettier eslint-plugin-prettier`
- Lint all files using `npx eslint . --fix`
