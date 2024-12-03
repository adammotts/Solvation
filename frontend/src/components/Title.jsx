import React from 'react';
import PropTypes from 'prop-types';

export function Title({ text }) {
  return <h1 style={styles.title}>{text}</h1>;
}

Title.propTypes = {
  text: PropTypes.string.isRequired,
};

const styles = {
  title: {
    textStroke: '1px black',
    color: 'white',
    fontSize: '50px',
    textShadow: `
      -1px -1px 0 black,
      1px -1px 0 black,
      -1px  1px 0 black,
      1px  1px 0 black
    `,
    margin: 0,
  },
};
