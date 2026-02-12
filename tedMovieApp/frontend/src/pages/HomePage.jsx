import React, { useEffect, useState } from "react";
import { useAuth } from "../auth/AuthContext";
import ReviewList from "../components/ReviewList";
import ReviewDetailModal from "../components/ReviewDetailModal";
import { fetchAllReviews, createReview } from "../services/api/reviewsApi";

function HomePage() {
  const { user, logout, isAuthenticated } = useAuth();

  // Prefer common claim names for display
  const displayName = user?.email || user?.unique_name || user?.name || "user";

  const [reviews, setReviews] = useState([]);
  const [selected, setSelected] = useState(null);
  const [status, setStatus] = useState("idle");
  const [error, setError] = useState("");

  // Create review form state
  const [movieId, setMovieId] = useState("");     // required by your endpoint as query param
  const [title, setTitle] = useState("");
  const [reviewText, setReviewText] = useState("");
  const [stars, setStars] = useState(5);
  const [createBusy, setCreateBusy] = useState(false);
  const [createError, setCreateError] = useState("");
  const [createSuccess, setCreateSuccess] = useState("");

  // Load all reviews on mount
  useEffect(() => {
    let cancelled = false;

    (async () => {
      setStatus("loading");
      setError("");
      try {
        const data = await fetchAllReviews(); // token auto-attached by httpClient
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

    return () => {
      cancelled = true;
    };
  }, []);

  async function handleCreateReview(e) {
    e.preventDefault();
    setCreateError("");
    setCreateSuccess("");

    if (!isAuthenticated) {
      setCreateError("You must be logged in to create a review.");
      return;
    }
    if (!movieId) {
      setCreateError("Please provide a movie ID.");
      return;
    }

    try {
      setCreateBusy(true);
      const newReview = await createReview(Number(movieId), {
        title,
        reviewText,
        stars: Number(stars),
      });

      // Optimistically add to the list (or you can re-fetch)
      setReviews((prev) => [newReview, ...prev]);

      setCreateSuccess("Review created!");
      setTitle("");
      setReviewText("");
      setStars(5);
    } catch (err) {
      console.error(err);
      // If backend returned message, it’s included in the thrown Error text
      setCreateError(
        err?.message || "Failed to create review. Check you are logged in and try again."
      );
    } finally {
      setCreateBusy(false);
    }
  }

  return (
    <div className="home" style={{ display: "grid", gap: 16 }}>
      <header className="home-header" style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <h1>My Reviews</h1>
        <div className="home-actions" style={{ display: "flex", gap: 12, alignItems: "center" }}>
          <span className="username">
            Logged in as <strong>{displayName}</strong>
          </span>
          <button onClick={logout}>Log out</button>
        </div>
      </header>

      {/* Create Review Form */}
      <section style={{ border: "1px solid #ddd", borderRadius: 8, padding: 16 }}>
        <h2 style={{ marginTop: 0 }}>Create a Review</h2>
        {!isAuthenticated && (
          <div style={{ color: "crimson", marginBottom: 8 }}>
            You are not logged in. Please log in to submit a review.
          </div>
        )}
        <form onSubmit={handleCreateReview} style={{ display: "grid", gap: 10, maxWidth: 520 }}>
          <label style={{ display: "grid", gap: 4 }}>
            <span>Movie ID</span>
            <input
              type="number"
              min="1"
              required
              value={movieId}
              onChange={(e) => setMovieId(e.target.value)}
              placeholder="e.g. 123"
            />
          </label>

          <label style={{ display: "grid", gap: 4 }}>
            <span>Title</span>
            <input
              type="text"
              required
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              placeholder="Great movie!"
            />
          </label>

          <label style={{ display: "grid", gap: 4 }}>
            <span>Review text</span>
            <textarea
              required
              value={reviewText}
              onChange={(e) => setReviewText(e.target.value)}
              placeholder="Write what you thought..."
              rows={4}
            />
          </label>

          <label style={{ display: "grid", gap: 4, maxWidth: 120 }}>
            <span>Stars (1–5)</span>
            <input
              type="number"
              min="1"
              max="5"
              value={stars}
              onChange={(e) => setStars(e.target.value)}
            />
          </label>

          <div style={{ display: "flex", gap: 8 }}>
            <button type="submit" disabled={!isAuthenticated || createBusy}>
              {createBusy ? "Submitting..." : "Submit Review"}
            </button>
            <button
              type="button"
              onClick={() => {
                setMovieId("");
                setTitle("");
                setReviewText("");
                setStars(5);
                setCreateError("");
                setCreateSuccess("");
              }}
            >
              Clear
            </button>
          </div>

          {createError && <div style={{ color: "crimson" }}>{createError}</div>}
          {createSuccess && <div style={{ color: "green" }}>{createSuccess}</div>}
        </form>
      </section>

      {/* Reviews List */}
      <section>
        {status === "loading" && <p>Loading reviews…</p>}
        {status === "error" && <div style={{ color: "crimson" }}>{error}</div>}
        {status === "success" && (
          <ReviewList reviews={reviews} onSelect={setSelected} />
        )}
      </section>

      <ReviewDetailModal review={selected} onClose={() => setSelected(null)} />
    </div>
  );
}

export default HomePage;