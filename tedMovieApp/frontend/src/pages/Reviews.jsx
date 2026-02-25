import { useEffect, useState } from "react";
import { useAuth } from "../auth/AuthContext";
import ReviewsListSection from "../features/reviews/ReviewsListSection";
import ReviewDetailModal from "../components/ReviewDetailModal";
import { fetchAllReviewsUser } from "../services/api/reviewsApi";

export default function MyReviewsPage() {
  const [reviews, setReviews] = useState([]);
  const [status, setStatus] = useState("idle");
  const [error, setError] = useState(null);
  const [selected, setSelected] = useState(null);

  const { token, isAuthenticated } = useAuth();

  useEffect(() => {
  if (!isAuthenticated || !token) return;

  let cancelled = false;

  (async () => {
    try {
      setStatus("loading");
      setError(null);

      // res IS the parsed JSON
      const data = await fetchAllReviewsUser();

      if (!cancelled) {
        setReviews(Array.isArray(data) ? data : [data]);
        setStatus("success");
      }
    } catch (e) {
      if (!cancelled) {
        setError(e.message || "Unknown error");
        setStatus("error");
      }
    }
  })();

  return () => {
    cancelled = true;
  };
}, [isAuthenticated, token]);

  if (!isAuthenticated) {
    return (
      <div style={{ padding: 16 }}>
        <h1>My Reviews</h1>
        <p>Please log in to view your reviews.</p>
      </div>
    );
  }

  return (
    <div style={{ display: "grid", gap: 16 }}>
      <h1>My Reviews</h1>

      <ReviewsListSection
        reviews={reviews}
        status={status}
        error={error}
        onSelect={setSelected}
        onDelete={null}
      />

      <ReviewDetailModal
        review={selected}
        onClose={() => setSelected(null)}
      />
    </div>
  );
}