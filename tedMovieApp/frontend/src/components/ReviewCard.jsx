import React from "react";

function Stars({ value = 0 }) {
  const filled = "★".repeat(value);
  const empty = "☆".repeat(Math.max(0, 5 - value));
  return <span aria-label={`${value} out of 5 stars`} title={`${value}/5`}>{filled}{empty}</span>;
}

function preview(text, max = 140) {
  if (!text) return "";
  return text.length > max ? text.slice(0, max).trim() + "…" : text;
}

function ReviewCard({ review, onClick }) {
  return (
    <button
      className="review-card"
      onClick={() => onClick?.(review)}
      aria-label={`Open review: ${review.title}`}
    >
      <div className="review-card__header">
        <h3 className="review-card__title">{review.title}</h3>
        <Stars value={review.stars} />
      </div>
      <p className="review-card__preview">{preview(review.reviewText)}</p>
    </button>
  );
}

export default ReviewCard;