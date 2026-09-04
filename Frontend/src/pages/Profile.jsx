import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import API, { getUserFromToken } from "../services/api";

export default function Profile() {
  const [user, setUser] = useState(null);
  const [stats, setStats] = useState({
    taskCount: 0,
    completedCount: 0,
  });

  const navigate = useNavigate();

  useEffect(() => {
    const loadProfile = async () => {
      try {
        const [profileResponse, tasksResponse] = await Promise.all([
          API.get("/Auth/profile"),
          API.get("/Task"),
        ]);

        const tokenUser = getUserFromToken();

        const tasks = tasksResponse.data || [];

        setUser({
          ...profileResponse.data,
          role: tokenUser?.isAdmin ? "Admin" : "User",
        });

        setStats({
          taskCount: tasks.length,
          completedCount: tasks.filter((task) => Number(task.status) === 3)
            .length,
        });
      } catch (error) {
        toast.error("Could not load profile.");
      }
    };

    loadProfile();
  }, []);

  const logout = () => {
    localStorage.removeItem("token");

    toast.success("Logged out successfully.");

    navigate("/login");
  };

  if (!user) {
    return <div className="loading">Loading profile...</div>;
  }

  const initials =
    `${user.firstName?.[0] || ""}${user.lastName?.[0] || ""}`.toUpperCase();

  const roleStyle =
    user.role === "Admin" ? "profile-role admin" : "profile-role user";

  return (
    <div className="profile-page">
      {/* Header */}
      <div className="profile-page-header">
        <h2>Profile</h2>
        <p>Manage your account information</p>
      </div>

      {/* Avatar + Name */}
      <section className="profile-user-card card">
        <div className="profile-user-content">
          <div className="profile-avatar">{initials}</div>

          <div className="profile-user-details">
            <h3>
              {user.firstName} {user.lastName}
            </h3>

            <p>{user.email}</p>

            <span className={roleStyle}>{user.role}</span>
          </div>
        </div>
      </section>

      {/* Account Information */}
      <section className="profile-info-card card">
        <div className="profile-section-header">
          <h4>Account Information</h4>
        </div>

        <div className="profile-info-list">
          <div className="profile-info-row">
            <span>First Name</span>
            <strong>{user.firstName || "-"}</strong>
          </div>

          <div className="profile-info-row">
            <span>Last Name</span>
            <strong>{user.lastName || "-"}</strong>
          </div>

          <div className="profile-info-row">
            <span>Email Address</span>
            <strong>{user.email || "-"}</strong>
          </div>

          <div className="profile-info-row">
            <span>Role</span>
            <strong>{user.role}</strong>
          </div>
        </div>
      </section>

      {/* Stats */}
      <div className="profile-stats">
        <div className="profile-stat-card card">
          <div className="profile-stat-value">{stats.taskCount}</div>

          <div className="profile-stat-label">
            {user.role === "Admin" ? "Total Tasks" : "My Tasks"}
          </div>
        </div>

        <div className="profile-stat-card card">
          <div className="profile-stat-value completed">
            {stats.completedCount}
          </div>

          <div className="profile-stat-label">Completed</div>
        </div>
      </div>

      {/* Sign Out */}
      <section className="profile-logout-card card">
        <h4>Sign Out</h4>

        <p>You will be signed out of your account on this device.</p>

        <button className="btn btn-danger" onClick={logout}>
          <span>↪</span>
          Sign Out
        </button>
      </section>
    </div>
  );
}
