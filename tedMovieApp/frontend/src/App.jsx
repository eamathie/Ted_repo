import React from "react";
import { Routes, Route, Link, Navigate } from "react-router-dom";
import RegistrationForm from "./components/RegistrationForm";
import LoginForm from "./components/LoginForm";
import HomePage from "./pages/HomePage";
import ProtectedRoute from "./auth/ProtectedRoute";
import "./App.css";

function App() {
  const handleRegistration = (data) => {
    console.log("Registration (App received):", { username: data.username });
  };

  const handleLogin = (data) => {
    //in real app when connected to backend, authenticate via backend and handle tokens 
    console.log("Login (App received):", { username: data.username });
  };

  return (
    <div className="App">
      {/* Simple nav for testing (optional) */}
      <nav style={{ display: "flex", gap: "1rem", marginBottom: "1rem" }}>
        <Link to="/login">Login</Link>
        <Link to="/register">Register</Link>
        <Link to="/">Home</Link>
      </nav>

      <Routes>
        {/* Default route → /login */}
        <Route path="/" element={
          <ProtectedRoute>
            <HomePage />
          </ProtectedRoute>
        } />

        <Route path="/login" element={<LoginForm onSubmit={handleLogin} />} />

        <Route
          path="/register"
          element={<RegistrationForm onSubmit={handleRegistration} showThankYou={true} />}
        />

        {/* If someone hits an unknown path, redirect to /login */}
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    </div>
  );
}

export default App;