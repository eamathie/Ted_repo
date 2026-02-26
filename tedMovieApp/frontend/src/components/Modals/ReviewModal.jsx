import React, { useState } from "react";
import Modal from "./Modal";
import CreateReviewSection from "../../features/reviews/CreateReviewSection";
import styles from "./ReviewModal.module.css";

function ReviewModal() {
     const [isModalOpen, setIsModalOpen] = useState(false);

  return (
    <div>
      <button className={styles.btnCreateReview} onClick={() => setIsModalOpen(true)}>+ Create New Review</button>

      <Modal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)}>
        <CreateReviewSection
      />
      </Modal>
    </div>
  );
}

export default ReviewModal;