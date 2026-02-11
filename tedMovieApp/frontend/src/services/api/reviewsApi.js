import { httpGet } from "../httpClient";

export function fetchAllReviews() {
  return httpGet("/api/reviews");
}