import React from 'react';
import PropTypes from 'prop-types';
import './Subtitle.css';

export function Subtitle({ text }) {
  return <h1 className="subtitle">{text}</h1>;
}

Subtitle.propTypes = {
  text: PropTypes.string.isRequired,
};
