import { NavLink, useNavigate } from "react-router-dom";
import toast from "react-hot-toast";

export default function Sidebar({ open, onClose, user }) {
  const navigate = useNavigate();

  const isAdmin = user?.isAdmin;

  const logout = () => {
    localStorage.removeItem("token");
    toast.success("Logged out successfully");
    navigate("/login");
  };

  const linkClass = ({ isActive }) => `nav-link ${isActive ? "active" : ""}`;

  return (
    <>
      {open && <div className="sidebar-overlay" onClick={onClose} />}

      <aside className={`sidebar ${open ? "open" : ""}`}>
        <div className="sidebar-brand">
          <span className="brand-icon">✓</span>
          <span>TaskFlow</span>
        </div>

        <nav className="nav">
          <NavLink to="/dashboard" className={linkClass} onClick={onClose}>
            <span>▦</span> Dashboard
          </NavLink>

          <NavLink to="/tasks" className={linkClass} onClick={onClose}>
            <span>✓</span>
            {isAdmin ? "All Tasks" : "My Tasks"}
          </NavLink>

          <NavLink to="/profile" className={linkClass} onClick={onClose}>
            <span>◯</span> Profile
          </NavLink>

          {isAdmin && (
            <div className="admin-badge">
              <span>ADMIN</span>
            </div>
          )}
        </nav>

        <button className="logout-button" onClick={logout}>
          <span>↪</span> Logout
        </button>
      </aside>
    </>
  );
}
