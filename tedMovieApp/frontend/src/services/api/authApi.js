// src/api/authApi.js
import { httpPost, setAuthToken, clearAuthToken } from "../httpClient";

export async function login(email, password) {
  const result = await httpPost("/api/auth/login", { email, password }); // <-- email key
  setAuthToken(result.token);
  return result;
}

export async function register(email, password) {
  return httpPost("/api/auth/register", { email, password }); // <-- email key
}

export function logout() {
  clearAuthToken();
}