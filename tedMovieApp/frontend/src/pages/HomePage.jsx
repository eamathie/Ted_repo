import React, { useEffect, useState } from "react";
import { useAuth } from "../auth/AuthContext";
import ReviewList from "../components/ReviewList";
import ReviewDetailModal from "../components/ReviewDetailModal";
import { fetchAllReviews } from "../services/reviewsApi";

function HomePage() {
  const { user, logout } = useAuth();
  const username = user?.username;

  const [reviews, setReviews] = useState([]);
  const [selected, setSelected] = useState(null);
  const [status, setStatus] = useState("idle"); // 'idle' | 'loading' | 'success' | 'error'
  const [error, setError] = useState("");

  useEffect(() => {
    if (!username) return;

    let cancelled = false;
    (async () => {
      setStatus("loading");
      setError("");
      try {
        const data = await fetchAllReviews();
        if (!cancelled) {
          setReviews(Array.isArray(data) ? data : []);
          setStatus("success");
        }
      } catch (e) {
        console.error(e);
        if (!cancelled) {
          setError(e.message || "Failed to load reviews");
          setStatus("error");
        }
      }
    })();

    return () => { cancelled = true; };
  }, [username]);

  return (
    <div className="home">
      <header className="home-header">
        <h1>My Reviews</h1>
        <div className="home-actions">
          <span className="username">
            Logged in as <strong>{username}</strong>
          </span>
          <button onClick={logout}>Log out</button>
        </div>
      </header>

      {status === "loading" && <p>Loading reviews…</p>}
      {status === "error" && <div style={{ color: "crimson" }}>{error}</div>}
      {status === "success" && (
        <ReviewList reviews={reviews} onSelect={setSelected} />
      )}

      <ReviewDetailModal review={selected} onClose={() => setSelected(null)} />
    </div>
  );
}

export default HomePage;