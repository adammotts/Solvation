import React from 'react';
import PropTypes from 'prop-types';
import { ModalButton } from '../../components';

export function Choices({ allMoves }) {
  function getMoveIcons(allMoves) {
    const images = [
      ['/images/correct_64x.png', 'Best Move'],
      ['/images/inaccuracy_64x.png', 'Inaccuracy'],
      ['/images/mistake_64x.png', 'Mistake'],
      ['/images/incorrect_64x.png', 'Blunder'],
    ];

    const sortedMoves = Object.entries(allMoves).sort((a, b) => b[1] - a[1]);

    const mappedMoves = {};
    const numMoves = sortedMoves.length;

    sortedMoves.forEach(([move, ev], index) => {
      if (index === 0) {
        mappedMoves[move] = {
          icon: images[0][0],
          label: images[0][1],
          name: move,
          ev: ev,
        };
      } else if (index === numMoves - 1) {
        mappedMoves[move] = {
          icon: images[3][0],
          label: images[3][1],
          name: move,
          ev: ev,
        };
      } else if (numMoves === 3 && index === 1) {
        mappedMoves[move] = {
          icon: images[2][0],
          label: images[2][1],
          name: move,
          ev: ev,
        };
      } else {
        mappedMoves[move] = {
          icon: images[index][0],
          label: images[index][1],
          name: move,
          ev: ev,
        };
      }
    });

    return mappedMoves;
  }

  const moves = getMoveIcons(allMoves);

  return (
    <>
      {Object.values(moves).map((move) => (
        <ModalButton key={move.label} move={move} />
      ))}
    </>
  );
}

Choices.propTypes = {
  allMoves: PropTypes.object.isRequired,
};
