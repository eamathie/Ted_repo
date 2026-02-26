// src/components/LoginForm.jsx
import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import styles from "./LoginForm.module.css";

export default function LoginForm() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("");        // <-- email, not username
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const onSubmit = async (e) => {
    e.preventDefault();
    setError("");
    try {
      await login(email, password);              // <-- calls backend with { email, password }
      navigate("/");
    } catch (err) {
      console.error(err);
      setError("Login failed. Check your credentials.");
    }
  };

  return (
    <div className={styles.container}>
    <form className={styles.loginForm} onSubmit={onSubmit} >
      <h2>Log in</h2>
      <input
        type="email"
        required
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        placeholder="Email"
        className={styles.inpEmail}
      />
      <input
        type="password"
        required
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        placeholder="Password"
        className={styles.inpPassword}
      />
      <button type="submit" className={styles.btnLogin}>Log in</button>
      {error && <div style={{ color: "crimson" }}>{error}</div>}

      <div style={{ marginTop: 12 }}>
        <span>Don’t have an account? </span>
        <Link to="/register">Register</Link>
      </div>
    </form>
    </div>
  );
}