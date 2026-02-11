const BASE_URL = import.meta.env.VITE_API_BASE_URL || "http://localhost:5059";

let _token = null;

export function setAuthToken(token) {
  _token = token;
  localStorage.setItem("auth_token", token);
}

export function getAuthToken() {
  return _token ?? localStorage.getItem("auth_token");
}

export function clearAuthToken() {
  _token = null;
  localStorage.removeItem("auth_token");
}

async function request(path, { method = "GET", body, headers = {} } = {}) {
  const token = getAuthToken();

  const mergedHeaders = {
    Accept: "application/json",
    ...(body ? { "Content-Type": "application/json" } : {}),
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
    ...headers,
  };

  const resp = await fetch(`${BASE_URL}${path}`, {
    method,
    headers: mergedHeaders,
    body: body ? JSON.stringify(body) : undefined,
  });

  if (!resp.ok) {
    const text = await resp.text().catch(() => "");
    if (resp.status === 401) {
      clearAuthToken();
    }
    throw new Error(`${method} ${path} failed: ${resp.status} ${text}`);
  }

  if (resp.status === 204) return null;

  const ct = resp.headers.get("content-type") || "";
  return ct.includes("application/json") ? resp.json() : resp.text();
}

export const httpGet    = (path, options={}) => request(path, { method: "GET", ...options });
export const httpPost   = (path, body, options={}) => request(path, { method: "POST", body, ...options });
export const httpPut    = (path, body, options={}) => request(path, { method: "PUT", body, ...options });
export const httpDelete = (path, options={}) => request(path, { method: "DELETE", ...options });