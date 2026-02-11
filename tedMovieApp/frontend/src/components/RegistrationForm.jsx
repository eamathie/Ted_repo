import React, { useState } from "react";
import { Link } from "react-router-dom";
import InputField from "./InputField";

function RegistrationForm({
  onSubmit,
  initialValues = { username: "", password: "" },
  showThankYou = true,
}) {
  const [submitted, setSubmitted] = useState(false);
  const [formData, setFormData] = useState({
    username: initialValues.username ?? "",
    password: initialValues.password ?? "",
  });

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    setSubmitted(true);

    console.log("Registration complete");
    console.log("Submitted username: ", formData.username);

    if (typeof onSubmit === "function") {
      onSubmit(formData);
    }
  };

  const inputFields = [
    { name: "username", type: "text", label: "Username" },
    { name: "password", type: "password", label: "Password" },
  ];

  if (showThankYou && submitted) {
    return (
      <div className="registration-form">
        <h2>Thank you for registering on our page {formData.username}.</h2>
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
    </form>
  );
}

export default RegistrationForm;