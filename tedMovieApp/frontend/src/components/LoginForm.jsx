import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import InputField from "./InputField";

/**
 * LoginForm
 * - Props (all optional):
 *    onSubmit: (formData) => void
 *    initialValues: { username?: string, password?: string }
 *
 * Behavior:
 *  - No thank-you screen
 *  - On submit, navigates to Home ('/')
 */
function LoginForm({
  onSubmit,
  initialValues = { username: "", password: "" },
}) {
  const navigate = useNavigate();
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

    // In real apps: validate, call API, show errors, etc.
    console.log("Login submitted (username only):", formData.username);

    if (typeof onSubmit === "function") {
      onSubmit(formData);
    }

    // Navigate to home page after "successful" login
    navigate("/");
  };

  const isDisabled = !formData.username || !formData.password;

  return (
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
  );
}

export default LoginForm;
