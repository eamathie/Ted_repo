// src/services/api/reviewsApi.js
import { httpGet, httpPost, httpDelete } from "../httpClient";

/** GET /api/reviews */
export function fetchAllReviews() {
  return httpGet("/api/reviews");
}

/** POST /api/reviews?movieId=123 with body { title, reviewText, stars } */
export function createReview(movieId, { title, reviewText, stars }) {
  if (movieId == null || movieId === "") {
    return Promise.reject(new Error("movieId is required"));
  }
  return httpPost(`/api/reviews?movieId=${encodeURIComponent(movieId)}`, {
    title,
    reviewText,
    stars,
  });
}

/** DELETE /api/reviews/{id} */
export function deleteReview(id) {
  if (!id) return Promise.reject(new Error("id is required"));
  return httpDelete(`/api/reviews/${id}`);
}
