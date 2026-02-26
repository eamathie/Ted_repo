import React, { useState } from "react";
import { useAuth } from "../auth/AuthContext";
import { useReviews } from "../features/reviews/hooks/useReviews";
import CreateReviewSection from "../features/reviews/CreateReviewSection";
import ReviewsListSection from "../features/reviews/ReviewsListSection";
import ReviewDetailModal from "../components/ReviewDetailModal";
import styles from "../components/Modals/ReviewModal.module.css";
import Modal from "../components/Modals/Modal";

export default function HomePage() {
  const { user, logout, isAuthenticated } = useAuth();
  const displayName = user?.email || user?.unique_name || user?.name || "user";

  const { reviews, status, error, create, remove } = useReviews();
  const [selected, setSelected] = useState(null);

  function ReviewModal() {
    const [isModalOpen, setIsModalOpen] = useState(false);

    return (
      <div>
        <button className={styles.btnCreateReview} onClick={() => setIsModalOpen(true)}>+ Create New Review</button>

        <Modal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)}>
          <CreateReviewSection
            isAuthenticated={isAuthenticated}
            onCreate={create}
          />
        </Modal>
      </div>
    );
  }

  return (
    <div className="home" style={{ display: "grid", gap: 16 }}>
      <header style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <h1>Reviews</h1>
      </header>

      <ReviewModal />

      <ReviewsListSection
        reviews={reviews}
        status={status}
        error={error}
        onSelect={setSelected}
        onDelete={remove}
      />

      <ReviewDetailModal
        review={selected}
        onClose={() => setSelected(null)}
      />
    </div>
  );
}