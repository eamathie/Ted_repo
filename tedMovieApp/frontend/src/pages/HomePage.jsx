import React, { useEffect, useState } from "react";
import { useAuth } from "../auth/AuthContext";
import ReviewList from "../components/ReviewList";
import ReviewDetailModal from "../components/ReviewDetailModal";
import { getReviews, seedDemoReviews } from "../services/reviewStorage";

function HomePage() {
  const { user, logout } = useAuth();
  const username = user?.username;

  const [reviews, setReviews] = useState([]);
  const [selected, setSelected] = useState(null);

  useEffect(() => {
    if (!username) {
      console.warn("HomePage: username is missing; are you logged in?");
      return;
    }

    // 1) Try to load reviews
    const existing = getReviews(username);
    console.log("HomePage: existing reviews", existing);

    // 2) If none exist, seed demo data
    if (!existing || existing.length === 0) {
      const seeded = seedDemoReviews(username);
      console.log("HomePage: seeded reviews", seeded);
      setReviews(seeded);
    } else {
      setReviews(existing);
    }
  }, [username]);

  return (
    <div className="home">
      <header className="home-header">
        <h1>My Reviews</h1>
        <div className="home-actions">
          <span className="username">
            Logged in as <strong>{username}</strong>
          </span>
          <button onClick={logout}>Log out</button>
        </div>
      </header>

      <ReviewList reviews={reviews} onSelect={setSelected} />
      <ReviewDetailModal review={selected} onClose={() => setSelected(null)} />
    </div>
  );
}

export default HomePage;