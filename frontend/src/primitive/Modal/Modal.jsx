import React from 'react';
import PropTypes from 'prop-types';
import './Modal.css';

export function Modal({ isOpen, onClose, children }) {
  if (!isOpen) return null;

  return (
    <>
      <div className="modal-overlay"></div>
      <div className="modal">
        <button className="modal-close-button" onClick={onClose}>
          ✖
        </button>
        <div className="modal-content">{children}</div>
      </div>
    </>
  );
}

Modal.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  children: PropTypes.node.isRequired,
};
