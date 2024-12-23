import React from 'react';
import PropTypes from 'prop-types';
import { ModalButton } from '../../components';
import './Choices.css';

export function Choices({ allMoves, onSelect, afterMove, choice }) {
  const moveOrder = ['hit', 'stand', 'double', 'split'];

  function getMoveIcons(allMoves) {
    const images = [
      { source: '/images/correct_64x.png', label: 'Best Move' },
      { source: '/images/inaccuracy_64x.png', label: 'Inaccuracy' },
      { source: '/images/mistake_64x.png', label: 'Mistake' },
      { source: '/images/incorrect_64x.png', label: 'Blunder' },
    ];

    const sortedMoves = Object.entries(allMoves)
      .filter(([, value]) => value !== null)
      .sort((a, b) => b[1] - a[1]);

    const mappedMoves = {};
    const numMoves = sortedMoves.length;

    sortedMoves.forEach(([move, ev], index) => {
      if (index === 0) {
        mappedMoves[move] = {
          ...images[0],
          name: move,
          ev: ev,
        };
      } else if (index === numMoves - 1) {
        mappedMoves[move] = {
          ...images[3],
          name: move,
          ev: ev,
        };
      } else if (numMoves === 3 && index === 1) {
        mappedMoves[move] = {
          ...images[2],
          name: move,
          ev: ev,
        };
      } else {
        mappedMoves[move] = {
          ...images[index],
          name: move,
          ev: ev,
        };
      }
    });

    return mappedMoves;
  }

  const moves = getMoveIcons(allMoves);

  return (
    <div className="moves-container">
      {Object.values(moves)
        .sort((a, b) => moveOrder.indexOf(a.name) - moveOrder.indexOf(b.name))
        .map((move) => (
          <ModalButton key={move.label} move={move} onSelect={onSelect} afterMove={afterMove} disabled={choice !== null && move.name !== choice} />
        ))}
    </div>
  );
}

Choices.propTypes = {
  allMoves: PropTypes.object.isRequired,
  onSelect: PropTypes.func.isRequired,
  afterMove: PropTypes.func.isRequired,
  choice: PropTypes.string,
};
