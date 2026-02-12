import { useCallback, useEffect, useState } from "react";
import { fetchAllReviews, createReview, deleteReview } from "../../../services/api/reviewsApi";

export function useReviews() {
  const [reviews, setReviews] = useState([]);
  const [status, setStatus] = useState("idle"); // idle | loading | success | error
  const [error, setError] = useState("");

  const refresh = useCallback(async () => {
    setStatus("loading");
    setError("");
    try {
      const data = await fetchAllReviews();
      setReviews(Array.isArray(data) ? data : []);
      setStatus("success");
    } catch (e) {
      setError(e?.message || "Failed to load reviews");
      setStatus("error");
    }
  }, []);

  useEffect(() => {
    refresh();
  }, [refresh]);

  const create = useCallback(async (movieId, payload) => {
    const created = await createReview(movieId, payload);
    // Always re-fetch to reflect DB
    await refresh();
    return created;
  }, [refresh]);

  const remove = useCallback(async (id) => {
    await deleteReview(id);
    await refresh();
  }, [refresh]);

  return { reviews, status, error, refresh, create, remove };
}