import { httpGet, httpPost } from "../httpClient";

export function fetchAllReviews() {
  return httpGet("/api/reviews");
}

export function createReview(movieId, { title, reviewText, stars }) {
  return httpPost(`/api/reviews?movieId=${movieId}`, {
    title,
    reviewText,
    stars
  });
}