import { Link } from "react-router-dom";
import "./Navbar.css";
import { useAuth } from "../auth/AuthContext";

export default function Navbar() {
  const { user, logout } = useAuth();

  return (
    <div className="topnav">
      <div className="topnav-left">
        <Link className="nav-item" to="/">Home</Link>
        <Link className="nav-item" to="/profile">Profile</Link>
        <Link className="nav-item" to="/my_reviews"> My reviews</Link>

        {/* ONLY show logout when user exists */}
        {user && (
          <button className="nav-item" onClick={logout}>
            Log out
          </button>
        )}
      </div>

      <div className="topnav-right" />
    </div>
  );
}


