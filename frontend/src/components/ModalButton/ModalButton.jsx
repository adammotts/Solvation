import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { Button, Modal } from '../../primitive';

export function ModalButton({ buttonText, playedMove, allMoves }) {
  const [isModalOpen, setModalOpen] = useState(false);

  const handleOpenModal = () => setModalOpen(true);
  const handleCloseModal = () => setModalOpen(false);

  function getMoveIcons(allMoves) {
    const images = [
      '/images/correct_64x.png',
      '/images/inaccuracy_64x.png',
      '/images/mistake_64x.png',
      '/images/incorrect_64x.png',
    ];

    const sortedMoves = Object.entries(allMoves).sort((a, b) => b[1] - a[1]);

    const mappedMoves = {};
    const numMoves = sortedMoves.length;

    sortedMoves.forEach(([move, ev], index) => {
      if (index === 0) {
        mappedMoves[move] = {
          icon: images[0],
          ev: ev,
        };
      } else if (index === numMoves - 1) {
        mappedMoves[move] = {
          icon: images[3],
          ev: ev,
        };
      } else if (index === 1) {
        mappedMoves[move] = {
          icon: images[1],
          ev: ev,
        };
      } else {
        mappedMoves[move] = {
          icon: images[2],
          ev: ev,
        };
      }
    });

    return mappedMoves;
  }

  const moves = getMoveIcons(allMoves);

  const played = moves[playedMove];

  return (
    <>
      <Button text={buttonText} onClick={handleOpenModal} />

      <Modal isOpen={isModalOpen} onClose={handleCloseModal}>
        <img src={played.icon} alt={'choice'} />
      </Modal>
    </>
  );
}

ModalButton.propTypes = {
  buttonText: PropTypes.string.isRequired,
  playedMove: PropTypes.string.isRequired,
  allMoves: PropTypes.object.isRequired,
};
