import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { Button, Modal } from '../../primitive';

export function ModalButton({ buttonText, modalContent }) {
  const [isModalOpen, setModalOpen] = useState(false);

  const handleOpenModal = () => setModalOpen(true);
  const handleCloseModal = () => setModalOpen(false);

  return (
    <>
      <Button text={buttonText} onClick={handleOpenModal} />

      <Modal isOpen={isModalOpen} onClose={handleCloseModal}>
        {modalContent}
      </Modal>
    </>
  );
}

ModalButton.propTypes = {
  buttonText: PropTypes.string.isRequired,
  modalContent: PropTypes.node.isRequired,
};
