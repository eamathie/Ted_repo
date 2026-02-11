const BASE_URL = import.meta.env.VITE_API_BASE_URL || "http://localhost:5059";

export async function httpGet(path, options = {}) {
  const resp = await fetch(`${BASE_URL}${path}`, {
    method: "GET",
    headers: { Accept: "application/json", ...(options.headers || {}) },
    // If you will use cookie-based auth (+ CORS credentials) later, add:
    // credentials: "include",
  });
  if (!resp.ok) {
    const text = await resp.text().catch(() => "");
    throw new Error(`GET ${path} failed: ${resp.status} ${text}`);
  }
  return resp.json();
}