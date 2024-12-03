import React from 'react';
import PropTypes from 'prop-types';
import './Label.css';

export function Label({ text }) {
  return <h1 className="Label">{text}</h1>;
}

Label.propTypes = {
  text: PropTypes.string.isRequired,
};
