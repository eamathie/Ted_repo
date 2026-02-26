import React, { useState } from "react";
import ReactDOM from "react-dom";
import "./modal.css"; // Optional for styling

// Modal Component
function Modal({ isOpen, onClose, children }) {
  if (!isOpen) return null; // Don't render if modal is closed

  return ReactDOM.createPortal(
    <div className="modal-overlay" onClick={onClose}>
      <div
        className="modal-content"
        onClick={(e) => e.stopPropagation()} // Prevent closing when clicking inside
      >
        <button className="close-btn" onClick={onClose}>
          &times;
        </button>
        {children}
      </div>
    </div>,
    document.getElementById("modal-root") // Portal target
  );
}

export default Modal;
