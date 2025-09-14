#!/bin/bash

# Navigate to frontend directory
cd "$(dirname "$0")/../frontend"

# Check if npm is available
if ! command -v npm &> /dev/null; then
    echo "npm command not found. Skipping frontend formatting."
    exit 0
fi

# Check if node_modules exists
if [ ! -d "node_modules" ]; then
    echo "node_modules not found. Running npm install..."
    npm install
fi

# Run ESLint with autofix
echo "Running ESLint with autofix..."
if npm run lint; then
    echo "ESLint completed successfully."
else
    echo "ESLint found issues but attempted to fix them."
fi

# Run Prettier
echo "Running Prettier..."
if npm run format; then
    echo "Prettier completed successfully."
else
    echo "Prettier completed with some issues."
fi

exit 0
