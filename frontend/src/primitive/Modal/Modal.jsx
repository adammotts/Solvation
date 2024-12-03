import React from 'react';
import PropTypes from 'prop-types';
import './Modal.css';

export function Modal({ isOpen, onClose, includeClose = false, children }) {
  if (!isOpen) return null;

  return (
    <>
      <div className="modal-overlay" onClick={onClose}></div>
      <div className="modal">
        {includeClose && (
          <button className="modal-close-button" onClick={onClose}>
            ✖
          </button>
        )}
        <div className="modal-content">{children}</div>
      </div>
    </>
  );
}

Modal.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  includeClose: PropTypes.bool,
  children: PropTypes.node.isRequired,
};
