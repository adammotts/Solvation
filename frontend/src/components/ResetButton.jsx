import React, { useState } from 'react';

export function ResetButton() {
  const [isLoading, setIsLoading] = useState(false);

  const handleReset = async () => {
    setIsLoading(true);
    try {
      const response = await fetch('http://localhost:5256/game-states', {
        method: 'PUT',
      });

      if (response.ok) {
        alert('Game states successfully reset.');
      } else {
        alert(`Failed to reset: ${response.statusText}`);
      }
    } catch (error) {
      console.error('Error:', error);
      alert('An error occurred while trying to reset game states.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <button
      onClick={handleReset}
      disabled={isLoading}
      style={{
        borderRadius: '5px',
        padding: '10px',
        backgroundColor: isLoading ? 'gray' : 'green',
        color: 'white',
        cursor: isLoading ? 'not-allowed' : 'pointer',
      }}
    >
      {isLoading ? 'Processing...' : 'Reset Game States'}
    </button>
  );
}
