import React from "react";
import ReviewList from "../../components/ReviewList";

export default function ReviewsListSection({ reviews, status, error, onSelect, onDelete }) {
  return (
    <section>
      {status === "loading" && <p>Loading reviews…</p>}
      {status === "error" && <div style={{ color: "crimson" }}>{error}</div>}
      {status === "success" && (
        <>
          <ReviewList reviews={reviews} onSelect={onSelect} />
          {/* Optional: inline list with delete
          <ul style={{ listStyle: "none", padding: 0 }}>
            {reviews.map((r) => (
              <li key={r.id} style={{ display: "flex", gap: 8, alignItems: "center", padding: 8, border: "1px solid #ddd", borderRadius: 6, marginBottom: 8 }}>
                <button onClick={() => onSelect(r)} style={{ flex: 1, textAlign: "left" }}>
                  <strong>{r.title}</strong> — {r.stars}/5
                  <div style={{ opacity: 0.75 }}>{r.reviewText}</div>
                </button>
                <button onClick={() => onDelete?.(r.id)}>Delete</button>
              </li>
            ))}
          </ul>
          */}
        </>
      )}
    </section>
  );
}