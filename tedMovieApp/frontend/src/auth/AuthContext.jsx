import React, { createContext, useContext, useEffect, useMemo, useState } from "react";
import { login as apiLogin, register as apiRegister, logout as apiLogout } from "../services/api/authApi";
import { getAuthToken } from "../services/httpClient";

// Decode JWT payload (no external libs)
function parseJwt(token) {
  try {
    const base64 = token.split(".")[1].replace(/-/g, "+").replace(/_/g, "/");
    const json = decodeURIComponent(
      atob(base64)
        .split("")
        .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
        .join("")
    );
    return JSON.parse(json);
  } catch {
    return null;
  }
}

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [token, setToken] = useState(null);
  const [user, setUser]   = useState(null); // decoded JWT claims
  const isAuthenticated   = !!token;

  // Hydrate from localStorage on mount
  useEffect(() => {
    const t = getAuthToken();
    if (t) {
      setToken(t);
      setUser(parseJwt(t));
    }
  }, []);

  const login = async (email, password) => {
    const { token: t } = await apiLogin(email, password); // backend returns { token }
    setToken(t);
    setUser(parseJwt(t));
  };

  const register = async (email, password) => {
    await apiRegister(email, password);
    await login(email, password); // optional auto-login
  };

  const logout = () => {
    apiLogout(); // clears localStorage via httpClient
    setToken(null);
    setUser(null);
  };

  const value = useMemo(
    () => ({
      token,
      user,              // includes claims like email, sub, role
      isAuthenticated,
      login,
      register,
      logout,
      hasRole: (role) =>
        !!user?.role && (Array.isArray(user.role) ? user.role.includes(role) : user.role === role),
    }),
    [token, user, isAuthenticated]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within <AuthProvider>");
  return ctx;
}