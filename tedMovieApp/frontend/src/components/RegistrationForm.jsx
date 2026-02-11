// src/components/RegistrationForm.jsx
import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import InputField from "./InputField";
import { useAuth } from "../auth/AuthContext"; // to auto-login
// Alternatively, you can import register from authApi directly:
// import { register as apiRegister } from "../api/authApi";

function RegistrationForm({
  initialValues = { email: "", password: "" },
  showThankYou = true,
}) {
  const navigate = useNavigate();
  const { register } = useAuth(); // calls backend and then auto-login (per our AuthContext)

  const [submitted, setSubmitted] = useState(false);
  const [formData, setFormData] = useState({
    email: initialValues.email ?? "",
    password: initialValues.password ?? "",
  });
  const [error, setError] = useState("");

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");
    try {
      // This calls backend: POST /api/auth/register { email, password }
      // and then auto-logs in (per our earlier AuthContext implementation).
      await register(formData.email, formData.password);
      setSubmitted(true);
      // If you do NOT want auto-login, call apiRegister + navigate to /login.
      // await apiRegister(formData.email, formData.password);
      // setSubmitted(true);
      // navigate("/login");
    } catch (e) {
      console.error(e);
      setError("Registration failed. Check requirements (password complexity, unique email).");
    }
  };

  const inputFields = [
    { name: "email", type: "text", label: "Email" },
    { name: "password", type: "password", label: "Password" },
  ];

  if (showThankYou && submitted) {
    return (
      <div className="registration-form">
        <h2>Thank you for registering, {formData.email}.</h2>
        <p>You can now proceed to login.</p>
        <Link to="/login">
          <button type="button">Go to Login</button>
        </Link>
      </div>
    );
  }

  return (
    <form className="registration-form" onSubmit={handleSubmit}>
      <h2>Register</h2>
      {inputFields.map((inputField) => (
        <InputField
          key={inputField.name}
          name={inputField.name}
          type={inputField.type}
          label={inputField.label}
          value={formData[inputField.name]}
          onChange={handleChange}
        />
      ))}
      <button type="submit">Register</button>
      {error && <div style={{ color: "crimson" }}>{error}</div>}
    </form>
  );
}

export default RegistrationForm;