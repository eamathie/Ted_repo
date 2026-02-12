import React, { useState } from "react";
import { createReview } from "../services/api/reviewsApi";

export default function CreateReviewForm({ movieId, onCreated }) {
  const [title, setTitle] = useState("");
  const [reviewText, setReviewText] = useState("");
  const [stars, setStars] = useState(5);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    try {
      const result = await createReview(movieId, {
        title,
        reviewText,
        stars: Number(stars),
      });

      setSuccess("Review created!");
      setTitle("");
      setReviewText("");
      setStars(5);

      if (onCreated) onCreated(result);
    } catch (err) {
      console.error(err);
      setError("Failed to create review. Are you logged in?");
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ display: "grid", gap: 10, maxWidth: 400 }}>
      <h3>Create Review</h3>

      <input
        type="text"
        placeholder="Review title"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        required
      />

      <textarea
        placeholder="Write your review..."
        value={reviewText}
        onChange={(e) => setReviewText(e.target.value)}
        required
      />

      <input
        type="number"
        min="1"
        max="5"
        value={stars}
        onChange={(e) => setStars(e.target.value)}
      />

      <button type="submit">Submit Review</button>

      {error && <div style={{ color: "crimson" }}>{error}</div>}
      {success && <div style={{ color: "green" }}>{success}</div>}
    </form>
  );
}