import React, { useState } from "react";
import { useAuth } from "../auth/AuthContext";
import { useReviews } from "../features/reviews/hooks/useReviews";
import CreateReviewSection from "../features/reviews/CreateReviewSection";
import ReviewsListSection from "../features/reviews/ReviewsListSection";
import ReviewDetailModal from "../components/ReviewDetailModal";
import "./HomePage.css";

export default function HomePage() {
  const { user, logout, isAuthenticated } = useAuth();
  const displayName = user?.email || user?.unique_name || user?.name || "user";

  const { reviews, status, error, create, remove } = useReviews();
  const [selected, setSelected] = useState(null);

  return (
    <div className="home">
      <h1>My Reviews</h1>
      <header className="home-header">
        
        <div className="user-info">
          Logged in as <strong>{displayName}</strong>
          <button onClick={logout}>Log out</button>
        </div>
      </header>

      <CreateReviewSection
        isAuthenticated={isAuthenticated}
        onCreate={create}
      />

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