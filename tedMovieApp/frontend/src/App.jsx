import React from "react";
import { Routes, Route, Link } from "react-router-dom";
import RegistrationForm from "./components/RegistrationForm";
import LoginForm from "./components/LoginForm";
import HomePage from "./pages/HomePage";
import Header from "./components/Header";
import "./App.css";

function App() {
  const handleRegistration = (data) => {
    // In a real app, send 'data' to your backend API
    console.log("Registration (App received):", { username: data.username });
  };

  const handleLogin = (data) => {
    // In a real app, authenticate via backend and handle tokens
    console.log("Login (App received):", { username: data.username });
  };

  return (
    <div className="App">
      {/* Simple navigation for testing */}
      <nav style={{ display: "flex", gap: "1rem", marginBottom: "1rem" }}>
        <Link to="/">Home</Link>
        <Link to="/register">Register</Link>
        <Link to="/login">Login</Link>
      </nav>

      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route
          path="/register"
          element={
            <RegistrationForm
              onSubmit={handleRegistration}
              showThankYou={true}
            />
          }
        />
        <Route
          path="/login"
          element={<LoginForm onSubmit={handleLogin} />}
        />
      </Routes>
    </div>
  );
}

export default App;
