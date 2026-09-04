import { useNavigate } from "react-router-dom";

export default function Header({ title, user, onMenuToggle }) {
  const navigate = useNavigate();

  const first = user?.firstName?.[0] || "";
  const last = user?.lastName?.[0] || "";
  const initials = `${first}${last}`.toUpperCase() || "U";

  return (
    <header className="header">
      <button className="mobile-menu" onClick={onMenuToggle}>
        ☰
      </button>

      <div className="mobile-brand">
        <span className="brand-icon">✓</span>
        <b>TaskFlow</b>
      </div>

      <h1 className="page-title">{title}</h1>

      <button className="user-button" onClick={() => navigate("/profile")}>
        {user?.isAdmin && (
          <span className="text-xs font-semibold text-indigo-600">ADMIN</span>
        )}
        <span className="avatar">{initials}</span>

        <span className="user-name">
          {user?.firstName} {user?.lastName}
        </span>
      </button>
    </header>
  );
}
