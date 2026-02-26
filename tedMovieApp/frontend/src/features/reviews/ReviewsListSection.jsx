/*import React from "react";
import ReviewList from "../../components/ReviewList";

export default function ReviewsListSection({ reviews, status, error, onSelect, onDelete }) {
  return (
    <section>
      {status === "loading" && <p>Loading reviews…</p>}
      {status === "error" && <div style={{ color: "crimson" }}>{error}</div>}
      {status === "success" && (
        <>
          <ReviewList reviews={reviews} onSelect={onSelect} />
          {
          <ul style={{ listStyle: "none", padding: 0 }}>
            <ul>
  {reviews.map(r => (
    <li key={r.reviewId}>
      <button onClick={() => onSelect(r)}>{r.title}</button>
      <button onClick={() => onDelete?.(r.reviewId)}>Delete</button>
    </li>
  ))}
</ul>
          </ul>
          }
        </>
      )}
    </section>
  );
}*/

import React from "react";
import ReviewList from "../../components/ReviewList";

export default function ReviewsListSection({
  reviews,
  status,
  error,
  onSelect,
  onDelete,
}) {
  return (
    <section>
      {status === "loading" && <p>Loading reviews…</p>}
      {status === "error" && <div className="error">{error}</div>}
      {status === "success" && (
        <ReviewList
          reviews={reviews}
          onSelect={onSelect}
          onDelete={onDelete} // <-- make ReviewList render the delete button
        />
      )}
    </section>
  );
}