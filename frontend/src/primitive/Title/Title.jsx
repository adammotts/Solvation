import React from 'react';
import PropTypes from 'prop-types';
import './Title.css';

export function Title({ text }) {
  return <h1 className="title">{text}</h1>;
}

Title.propTypes = {
  text: PropTypes.string.isRequired,
};
