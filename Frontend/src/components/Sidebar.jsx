import { NavLink, useNavigate } from "react-router-dom";
import toast from "react-hot-toast";

export default function Sidebar({ open, onClose }) {
  const navigate = useNavigate();

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
            <span>✓</span> Tasks
          </NavLink>
          <NavLink to="/profile" className={linkClass} onClick={onClose}>
            <span>◯</span> Profile
          </NavLink>
        </nav>

        <button className="logout-button" onClick={logout}>
          <span>↪</span> Logout
        </button>
      </aside>
    </>
  );
}
