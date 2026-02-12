import React, { useState } from "react";
import { useMovieSearch } from "./hooks/useMovieSearch";

export default function CreateReviewSection({ isAuthenticated, onCreate }) {
  const [selectedMovie, setSelectedMovie] = useState(null);
  const [movieId, setMovieId] = useState("");
  const [title, setTitle] = useState("");
  const [reviewText, setReviewText] = useState("");
  const [stars, setStars] = useState(5);
  const [busy, setBusy] = useState(false);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const { query, setQuery, results, busy: searchBusy, error: searchError, setResults } = useMovieSearch(300);

  const handlePick = (m) => {
    setSelectedMovie(m);
    setMovieId(m.id);
    setQuery(m.title);
    setResults([]); // close list
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage("");
    setError("");

    if (!isAuthenticated) {
      setError("Please log in to create a review.");
      return;
    }
    if (!movieId || isNaN(Number(movieId))) {
      setError("Please select a movie from search (numeric ID).");
      return;
    }

    try {
      setBusy(true);
      await onCreate(Number(movieId), { title, reviewText, stars: Number(stars) });
      setMessage("Review created!");
      setTitle("");
      setReviewText("");
      setStars(5);
    } catch (err) {
      setError(err?.message || "Failed to create review");
    } finally {
      setBusy(false);
    }
  };

  return (
    <section style={{ border: "1px solid #ccc", padding: 16, borderRadius: 8, display: "grid", gap: 12 }}>
      <h2>Create Review</h2>

      {/* Movie search */}
      <div style={{ display: "grid", gap: 8 }}>
        <label>
          Search movie:
          <input
            type="text"
            placeholder="Type at least 2 letters..."
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
        </label>

        {searchBusy && <div>Searching…</div>}
        {searchError && <div style={{ color: "crimson" }}>{searchError}</div>}

        {results.length > 0 && (
          <div style={{ border: "1px solid #ddd", borderRadius: 6, background: "#fff", maxHeight: 240, overflowY: "auto" }}>
            {results.map((m) => (
              <button
                key={m.id}
                onClick={() => handlePick(m)}
                style={{ width: "100%", textAlign: "left", padding: 8, borderBottom: "1px solid #eee", display: "flex", gap: 8, alignItems: "center" }}
              >
                {m.posterUrl ? (
                  <img
                    src={m.posterUrl}
                    alt=""
                    width={28}
                    height={40}
                    style={{ objectFit: "cover", borderRadius: 2 }}
                    onError={(e) => (e.currentTarget.style.display = "none")}
                  />
                ) : (
                  <span style={{ width: 28 }} />
                )}
                <span style={{ flex: 1 }}>
                  {m.title}{m.releaseYear ? ` (${m.releaseYear})` : ""}
                </span>
                <code style={{ opacity: 0.7 }}>{m.id}</code>
              </button>
            ))}
          </div>
        )}

        {selectedMovie && (
          <div style={{ padding: 8, background: "#f7f7f7", borderRadius: 6, display: "grid", gap: 4 }}>
            <div>
              <strong>Selected:</strong> {selectedMovie.title}
              {selectedMovie.releaseYear ? ` (${selectedMovie.releaseYear})` : ""}
            </div>
            <div><strong>Movie ID:</strong> <code>{movieId}</code></div>
          </div>
        )}
      </div>

      {/* Create review form */}
      {error && <div style={{ color: "crimson" }}>{error}</div>}
      {message && <div style={{ color: "green" }}>{message}</div>}

      <form onSubmit={handleSubmit} style={{ display: "grid", gap: 10, maxWidth: 420 }}>
        <label>
          Title
          <input value={title} onChange={(e) => setTitle(e.target.value)} required />
        </label>

        <label>
          Review text
          <textarea rows={4} value={reviewText} onChange={(e) => setReviewText(e.target.value)} required />
        </label>

        <label>
          Stars (1–5)
          <input type="number" min="1" max="5" value={stars} onChange={(e) => setStars(e.target.value)} required />
        </label>

        <button type="submit" disabled={busy || !movieId || isNaN(Number(movieId))}>
          {busy ? "Creating…" : "Submit Review"}
        </button>
      </form>
    </section>
  );
}