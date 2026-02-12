import { Routes, Route, Link } from "react-router-dom";
import './Navbar.css';

export default function Navbar() {

  return (
    <div className="topnav">
      <a className="active" href="#home">Home</a>
      <a href="#profile">Profile</a>
    </div>
  );
}



