import React, { useState } from 'react';
import PropTypes from 'prop-types';

export function Button({ text, onClick }) {
  const [isClicked, setIsClicked] = useState(false);

  const handleMouseDown = () => setIsClicked(true);
  const handleMouseUp = () => setIsClicked(false);

  return (
    <button
      onClick={onClick}
      onMouseDown={handleMouseDown}
      onMouseUp={handleMouseUp}
      onMouseLeave={handleMouseUp}
      style={{
        ...styles.button,
        ...(isClicked && styles.buttonClicked),
      }}
    >
      <h1>{text}</h1>
    </button>
  );
}

Button.propTypes = {
  text: PropTypes.string.isRequired,
  onClick: PropTypes.func.isRequired,
};

const styles = {
  button: {
    borderRadius: '5px',
    padding: '0px 15px',
    color: 'white',
    fontSize: '10px',
    textShadow: `
      -1px -1px 0 black,
      1px -1px 0 black,
      -1px  1px 0 black,
      1px  1px 0 black
    `,
    backgroundColor: '#0B3D0B',
    border: '1px solid black',
    cursor: 'pointer',
    transition: 'transform 0.05s ease',
  },
  buttonClicked: {
    transform: 'scale(0.95)',
  },
};
