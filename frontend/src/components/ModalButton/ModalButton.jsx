import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { Text, Button, Modal } from '../../primitive';

export function ModalButton({ move, afterMove }) {
  const [isModalOpen, setModalOpen] = useState(false);

  const handleOpenModal = () => setModalOpen(true);
  const handleCloseModal = () => {
    setModalOpen(false);
    afterMove();
  };

  return (
    <>
      <Button text={move.name} onClick={handleOpenModal} />

      <Modal isOpen={isModalOpen} onClose={handleCloseModal}>
        <div>
          <img src={move.source} alt={move.label} />
          {move.label}
        </div>
        <Text
          text={`By choosing to ${move.name.toLowerCase()}, you have an expected value
          of ${move.ev.toFixed(4)}.`}
        />
      </Modal>
    </>
  );
}

ModalButton.propTypes = {
  move: PropTypes.object.isRequired,
  afterMove: PropTypes.func.isRequired,
};
