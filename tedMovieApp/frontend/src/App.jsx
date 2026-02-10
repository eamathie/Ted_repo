import { useState } from 'react'
import './App.css'
import InputField from './components/InputField'

function App() {
  const [submitted, setSubmitted] = useState(false);

  const [formData, setFormData] = useState({
    username: "",
    password: "",
  });

  const handleChange = (event) => {
    const {name, value} = event.target;

    setFormData(prev => ({
      ...prev,
      [name] : value,
    }));
  };


  const handleSubmit = (event) => {
    event.preventDefault();
    setSubmitted(true);
    console.log("Registration complete");
    console.log("Submitted values: ", formData);
  };

  const inputFields = [
    {name: "username", type: "text", label: "Username"},
    {name: "password", type: "password", label: "Password"},
  ];

  return (
      <div className = "App">
        {submitted ? (
          <h2> Thank you for regiserting on our page {formData.username}.</h2>
        ): (<form onSubmit = {handleSubmit}>
        {inputFields.map((inputField) => (
          <InputField
              key = {inputField.name}
              name = {inputField.name}
              type = {inputField.type}
              label = {inputField.label}
              value = {formData[inputField.name]}
              onChange = {handleChange}
              />
        ))}
        <button type = "submit"> Register </button>
        </form>
        )}

      </div>

  )
}

export default App

