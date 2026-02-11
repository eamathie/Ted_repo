import React, { useState } from "react";
import InputField from "./InputField";

/**
 * RegistrationForm
 * - Props (all optional):
 *    onSubmit: (formData) => void   // called after successful submit
 *    initialValues: { username?: string, password?: string }
 *    showThankYou: boolean          // if true, shows a thank-you screen after submit
 */

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
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    setSubmitted(true);

    // Avoid logging passwords in real apps; only log username if needed
    console.log("Registration complete");
    console.log("Submitted username: ", formData.username);

    if (typeof onSubmit === "function") {
      onSubmit(formData);
    }
  };

  const inputFields = [
    { name: "username", type: "text", label: "Username" },
    { name: "password", type: "password", label: "Password" }, // masked input
  ];

  if (showThankYou && submitted) {
    return (
      <div className="registration-form">
        <h2>Thank you for registering on our page {formData.username}.</h2>
      </div>
    );
  }

  return (
    <form className="registration-form" onSubmit={handleSubmit}>
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