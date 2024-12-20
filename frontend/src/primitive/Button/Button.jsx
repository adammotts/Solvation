import React, { useState } from 'react';
import { Loading } from '..';
import PropTypes from 'prop-types';
import './Button.css';

export function Button({ text, onClick, loading = false }) {
  const [isClicked, setIsClicked] = useState(false);

  const handleMouseDown = () => setIsClicked(true);
  const handleMouseUp = () => setIsClicked(false);

  return (
    <button
      className={`button ${isClicked ? 'button-clicked' : ''}`}
      onClick={onClick}
      onMouseDown={handleMouseDown}
      onMouseUp={handleMouseUp}
      onMouseLeave={handleMouseUp}
      disabled={loading}
    >
      {loading ? <Loading size={'25px'} /> : <h1>{text}</h1>}
    </button>
  );
}

Button.propTypes = {
  text: PropTypes.string.isRequired,
  onClick: PropTypes.func.isRequired,
  loading: PropTypes.bool,
};
