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
      <div>
        <div style={{ padding: 16 }}>
          <h1>My Reviews</h1>
          <p>Please log in to view your reviews.</p>
        </div>

        {/* <div className="skeleton-container">
          <div className="skeleton-item">
            <div className="movieTitle">Movie Title</div>
            <div className="reviewTitle">Movie Review Title</div>
              <div className="reviewText">Movie review paragraph</div>
            <div className="moviePoster">this is the movie poster</div>
          </div>
        </div> */}

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