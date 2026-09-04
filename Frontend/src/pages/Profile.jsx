import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import API from "../services/api";

export default function Profile() {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    API.get("/Auth/profile")
      .then((res) => setUser(res.data))
      .catch(() => toast.error("Could not load profile."));
  }, []);

  const logout = () => {
    localStorage.removeItem("token");
    toast.success("Logged out successfully.");
    navigate("/login");
  };

  if (!user) return <div className="loading">Loading profile...</div>;

  const initials =
    `${user.firstName?.[0] || ""}${user.lastName?.[0] || ""}`.toUpperCase();

  return (
    <section className="profile-card card">
      <div className="profile-avatar">{initials}</div>
      <h2>
        {user.firstName} {user.lastName}
      </h2>
      <p>{user.email}</p>
      <div className="profile-info">
        <div>
          <label>First Name</label>
          <strong>{user.firstName}</strong>
        </div>
        <div>
          <label>Last Name</label>
          <strong>{user.lastName}</strong>
        </div>
        <div>
          <label>Email</label>
          <strong>{user.email}</strong>
        </div>
      </div>
      <button className="btn btn-danger" onClick={logout}>
        Logout
      </button>
    </section>
  );
}
