import React from "react";
import { Routes, Route, Link, Navigate } from "react-router-dom";
import RegistrationForm from "./components/RegistrationForm";
import LoginForm from "./components/LoginForm";
import HomePage from "./pages/HomePage";
import ProtectedRoute from "./auth/ProtectedRoute";
import Navbar from "./components/Navbar"
import Header from "./components/Header";
import "./App.css";
import Footer from "./components/Footer";

function App() {
  return (
    <div className="App">
      <Header />
      <Navbar />
      <main>
        <Routes>
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <HomePage />
              </ProtectedRoute>
            }
          />
          <Route path="/login" element={<LoginForm />} />
          <Route
            path="/register"
            element={<RegistrationForm showThankYou={true} />}
          />
          <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
      </main>
      <Footer />

    </div>
  );
}

export default App;