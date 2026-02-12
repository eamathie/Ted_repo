import React, { useState } from "react";
import { useNavigate, useLocation, Link } from "react-router-dom";
import InputField from "./InputField";
import { useAuth } from "../auth/AuthContext";

/**
 * LoginForm
 * - On submit: sets auth, then navigates to Home ('/') or the originally requested page.
 */
function LoginForm({ initialValues = { username: "", password: "" }, onSubmit }) {
  const navigate = useNavigate();
  const location = useLocation();
  const { login } = useAuth();

  const [formData, setFormData] = useState({
    username: initialValues.username ?? "",
    password: initialValues.password ?? "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  
const handleSubmit = (e) => {
  e.preventDefault();

  if (typeof onSubmit === "function") {
    onSubmit(formData);
  }

  // Save the username in auth context (frontend-only)
  login({ username: formData.username });

  const dest = location.state?.from?.pathname || "/";
  navigate(dest, { replace: true });
};


  const isDisabled = !formData.username || !formData.password;

  return (
    <div className="main">
    <form className="login-form" onSubmit={handleSubmit}>
      <h2>Login</h2>

      <InputField
        name="username"
        type="text"
        label="Username"
        value={formData.username}
        onChange={handleChange}
      />
      <InputField
        name="password"
        type="password"
        label="Password"
        value={formData.password}
        onChange={handleChange}
      />

      <button type="submit" disabled={isDisabled}>Log in</button>

      <div style={{ marginTop: 12 }}>
        <span>Don’t have an account? </span>
        <Link to="/register">Register</Link>
      </div>
    </form>
    </div>
  );
}

export default LoginForm;