import React from "react";

function Stars({ value = 0 }) {
  const filled = "★".repeat(value);
  const empty = "☆".repeat(Math.max(0, 5 - value));
  return <span>{filled}{empty}</span>;
}

function ReviewDetailModal({ review, onClose }) {
  if (!review) return null;

  return (
    <div className="modal-overlay" role="dialog" aria-modal="true" onClick={onClose}>
      <div className="modal-card" onClick={(e) => e.stopPropagation()}>
        <header className="modal-header">
          <h2>{review.title}</h2>
          <button className="modal-close" onClick={onClose} aria-label="Close">×</button>
        </header>

        <div className="modal-meta">
          <Stars value={review.stars} /> <span>({review.stars}/5)</span>
        </div>

        <article className="modal-content">
          <p>{review.reviewText}</p>
        </article>

        {/* Optional metadata (movieId, etc.) */}
        {/* <small>Movie ID: {review.movieId}</small> */}
      </div>
    </div>
  );
}

export default ReviewDetailModal;