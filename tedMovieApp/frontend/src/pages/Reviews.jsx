import { useEffect, useState } from "react";
import { useAuth } from "../auth/AuthContext";
import ReviewsListSection from "../features/reviews/ReviewsListSection";
import ReviewDetailModal from "../components/ReviewDetailModal";

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

        const res = await fetch("/api/reviews/mine", {
          method: "GET",
          headers: {
            Accept: "application/json",
            Authorization: `Bearer ${token}`,
          },
        });

        if (!res.ok) {
          const text = await res.text();
          const msg = text || `Failed to load your reviews (${res.status})`;
          throw new Error(msg);
        }

        const data = await res.json();
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