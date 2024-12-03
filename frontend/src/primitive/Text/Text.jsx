import React from 'react';
import PropTypes from 'prop-types';
import './Text.css';

export function Text({ text }) {
  return <text className="text">{text}</text>;
}

Text.propTypes = {
  text: PropTypes.string.isRequired,
};
