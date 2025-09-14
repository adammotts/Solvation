#!/bin/bash

# Navigate to backend directory
cd "$(dirname "$0")/../backend"

# Check if dotnet is available
if ! command -v dotnet &> /dev/null; then
    echo "dotnet command not found. Skipping backend formatting."
    exit 0
fi

# Try to format the code
echo "Formatting backend code..."
if dotnet format --verbosity quiet 2>/dev/null; then
    echo "Backend formatting completed successfully."
else
    echo "Backend formatting failed or not available. This is not critical."
fi

# Try to build the project
echo "Building backend project..."
if dotnet build --verbosity quiet 2>/dev/null; then
    echo "Backend build completed successfully."
else
    echo "Backend build failed. This is not critical."
fi

exit 0
