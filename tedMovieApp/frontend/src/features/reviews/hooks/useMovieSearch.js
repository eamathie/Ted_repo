import { useEffect, useState } from "react";
import { searchMovies } from "../../../services/api/moviesApi";

export function useMovieSearch(delayMs = 300) {
  const [query, setQuery] = useState("");
  const [results, setResults] = useState([]);
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    setError("");

    const q = query.trim();
    if (q.length < 2) {
      setResults([]);
      return;
    }

    let cancelled = false;
    const t = setTimeout(async () => {
      try {
        setBusy(true);
        const list = await searchMovies(q);
        if (!cancelled) setResults(list);
      } catch (e) {
        if (!cancelled) setError("Search failed. Try again.");
      } finally {
        if (!cancelled) setBusy(false);
      }
    }, delayMs);

    return () => {
      cancelled = true;
      clearTimeout(t);
    };
  }, [query, delayMs]);

  return { query, setQuery, results, busy, error, setResults };
}