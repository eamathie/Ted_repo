import { Link, useNavigate } from "react-router-dom";
import "./Navbar.css";
import { useAuth } from "../auth/AuthContext";

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleAuthClick = () => {
    if (user) {
      logout();
    } else {
      navigate("/login");
    }
  };

  return (
    <div className="topnav">
      <div className="topnav-left">
        <Link className="nav-item" to="/">Home</Link>
        <Link className="nav-item" to="/profile">Profile</Link>
        <Link className="nav-item" to="/my_reviews"> My reviews</Link>

        {/* One control that changes label + behavior, same place */}
        <button className="nav-item" onClick={handleAuthClick}>
          {user ? "Log out" : "Log in"}
        </button>
      </div>

      {/* Keep or remove; empty right side keeps layout flexible */}
      <div className="topnav-right" />
    </div>
  );
}




