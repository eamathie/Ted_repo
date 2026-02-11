/*const STORAGE_KEY = "reviews_by_user";

/**
 * JS shape mirrors your C# fields (camelCase in JS):
 * { reviewId: number, title: string, reviewText: string, stars: 1|2|3|4|5, movieId: number }
 */
/*
function getStore() {
  try {
    return JSON.parse(localStorage.getItem(STORAGE_KEY) || "{}");
  } catch {
    return {};
  }
}

function setStore(obj) {
  localStorage.setItem(STORAGE_KEY, JSON.stringify(obj));
}

export function getReviews(username) {
  const store = getStore();
  return store[username] || [];
}

export function setReviews(username, reviews) {
  const store = getStore();
  store[username] = reviews;
  setStore(store);
}

/** Optional: seed demo reviews if user has none (handy for UI testing) */
/*export function seedDemoReviews(username) {
  const existing = getReviews(username);
  if (existing.length) return existing;

  const demo = [
    {
      reviewId: 1,
      title: "Inception",
      reviewText:
        "Inception is a film directed by Christopher Nolan that explores complex themes of dreams and reality. Roger Ebert describes it as a film that intricately weaves a narrative about planting an idea in someones mind, showcasing a perfectly oiled dream machine that captivates audiences on multiple viewings. According to Rotten Tomatoes, it is both intriguing and frustrating but ultimately satisfying, making it a triumph in filmmaking. User reviews on IMDb highlight its emotional depth and well-composed score, emphasizing that it stands out among typical action films. Overall, Inception is celebrated for its innovative storytelling and visual effects, making it a must-watch for cinephiles.",
      stars: 5,
      movieId: 42,
    },
    {
      reviewId: 2,
      title: "The Room",
      reviewText:
        "It's so bad it's good. An unintentional masterclass in how not to make a movie, yet oddly charming.",
      stars: 2,
      movieId: 99,
    },
  ];

  setReviews(username, demo);
  return demo;
}*/