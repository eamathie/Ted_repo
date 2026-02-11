import React from "react";
import "./App.css";
import RegistrationForm from "./components/RegistrationForm";

function App() {
  const handleRegistration = (data) => {
    // You can send 'data' to an API here
    // data = { username: '...', password: '...' }
    console.log("Form submitted (App received):", { username: data.username });
  };

  return (
    <div className="App">
      <RegistrationForm
        //onSubmit={handleRegistration}
      />
    </div>
  );
}

export default App;

