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

    return () => {
      cancelled = true;
    };
  }, [isAuthenticated, token]);

  if (!isAuthenticated) {
    return (
      <div style={{ padding: 16 }}>
        <h1>Profile</h1>
        <p>Please log in to view your profile.</p>
      </div>
    );
  }

  return (
    <div style={{ display: "grid", gap: 16 }}>
      <h1>Profile</h1>

      info
    </div>
  );
}