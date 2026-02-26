import React, { useState } from "react";
import ReactDOM from "react-dom";
import styles from "./Modal.module.css";

// Modal Component
function Modal({ isOpen, onClose, children }) {
  if (!isOpen) return null; // Don't render if modal is closed

  return ReactDOM.createPortal(
    <div className={styles.modalOverlay} onClick={onClose}>
      <div
        className={styles.modalContent}
        onClick={(e) => e.stopPropagation()} // Prevent closing when clicking inside
      >
        <button className={styles.closeBtn} onClick={onClose}>
          &times;
        </button>
        {children}
      </div>
    </div>,
    document.getElementById("modal-root") // Portal target
  );
}

export default Modal;
