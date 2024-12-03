import React from 'react';
import PropTypes from 'prop-types';

export function Subtitle({ text }) {
  return <h1 style={styles.subtitle}>{text}</h1>;
}

Subtitle.propTypes = {
  text: PropTypes.string.isRequired,
};

const styles = {
  subtitle: {
    color: 'white',
    fontSize: '30px',
    textShadow: `
      -1px -1px 0 black,
      1px -1px 0 black,
      -1px  1px 0 black,
      1px  1px 0 black
    `,
    margin: 0,
  },
};
