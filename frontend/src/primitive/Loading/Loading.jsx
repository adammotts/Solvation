import React from 'react';
import PropTypes from 'prop-types';
import './Loading.css';

export function Loading({ size = '100px' }) {
  return <div className="spinner" style={{ width: size, height: size }} />;
}

Loading.propTypes = {
  size: PropTypes.string,
};