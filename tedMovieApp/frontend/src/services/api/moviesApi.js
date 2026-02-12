// src/services/api/moviesApi.js
import { httpGet } from "../httpClient";

/**
 * Calls your backend: GET /api/movies/search?title=...
 * Normalizes common property names so the UI doesn't break if casing differs.
 */
export async function searchMovies(title) {
  const raw = await httpGet(`/api/movies/search?title=${encodeURIComponent(title)}`);

  const list = Array.isArray(raw) ? raw : [];
  return list
    .map((m) => ({
      // Prefer local numeric DB id if present:
      id: m.id ?? m.movieId ?? m.Id,
      title: m.title ?? m.Title,
      releaseYear: m.releaseYear ?? m.ReleaseYear ?? m.year ?? m.Year,
      posterUrl: m.posterUrl ?? m.PosterUrl ?? m.poster ?? m.Poster,
      raw: m, // keep original for debugging
    }))
    .filter((x) => x.id != null && x.title); // ensure we have what we need
}