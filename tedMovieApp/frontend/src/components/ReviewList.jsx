/*import React from "react";
import ReviewCard from "./ReviewCard";

function ReviewList({ reviews, onSelect }) {
  if (!reviews?.length) {
    return (
      <div className="no-reviews">
        <h2>You have no reviews</h2>
        <p>Start by adding your first review!</p>
      </div>
    );
  }

  return (
    <div className="reviews-grid">
      {reviews.map((r) => (
        <ReviewCard key={r.reviewId ?? `${r.title}-${r.movieId}`} review={r} onClick={onSelect} />
      ))}
    </div>
  );
}

export default ReviewList;*/

// src/components/ReviewList.jsx
import React from "react";
import ReviewCard from "./ReviewCard";

function ReviewList({ reviews, onSelect, onDelete }) {
  if (!reviews?.length) {
    return (
      <div className="no-reviews">
        <h2>You have no reviews</h2>
        <p>Start by adding your first review!</p>
      </div>
    );
  }

  return (
    <div className="reviews-grid">
      {reviews.map((r) => (
        <ReviewCard
          key={r.reviewId ?? `${r.title}-${r.movieId}`}  // adjust if your id differs
          review={r}
          onClick={onSelect}
          onDelete={onDelete}  // <-- pass through
        />
      ))}
    </div>
  );
}

export default ReviewList;