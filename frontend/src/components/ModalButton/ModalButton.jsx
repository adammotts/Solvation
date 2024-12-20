import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { Text, Button, Modal, Label } from '../../primitive';
import './ModalButton.css';

export function ModalButton({ move, afterMove }) {
  const [isModalOpen, setModalOpen] = useState(false);

  const handleOpenModal = () => setModalOpen(true);
  const handleCloseModal = () => {
    setModalOpen(false);
    setIsVisible(false);
    afterMove();
  };

  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    if (isModalOpen) {
      const timer = setTimeout(() => {
        setIsVisible(true);
      }, 500);
      return () => clearTimeout(timer);
    } else {
      setIsVisible(false);
    }
  }, [isModalOpen]);

  function capitalize(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
  }

  return (
    <>
      <Button text={capitalize(move.name)} onClick={handleOpenModal} />

      <Modal isOpen={isModalOpen} onClose={handleCloseModal}>
        <div className="label-container">
          <img src={move.source} alt={move.label} />
          <Label text={move.label} />
        </div>
        <Text
          text={`By choosing to ${move.name.toLowerCase()}, you have an expected value
          of ${move.ev.toFixed(4)}`}
        />
        <div className={`next-button-container ${isVisible ? 'visible' : ''}`}>
          <Button text={'Next'} onClick={handleCloseModal} />
        </div>
      </Modal>
    </>
  );
}

ModalButton.propTypes = {
  move: PropTypes.object.isRequired,
  afterMove: PropTypes.func.isRequired,
};
