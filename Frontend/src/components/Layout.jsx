import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import Header from "./Header";
import Sidebar from "./Sidebar";
import API from "../services/api";

const titles = {
  "/dashboard": "Dashboard",
  "/tasks": "Tasks",
  "/profile": "My Profile",
};

export default function Layout({ children }) {
  const [user, setUser] = useState(null);
  const [menuOpen, setMenuOpen] = useState(false);
  const location = useLocation();

  useEffect(() => {
    API.get("/Auth/profile")
      .then((res) => setUser(res.data))
      .catch(() => {});
  }, []);

  const title =
    titles[location.pathname] ||
    (location.pathname === "/tasks/new"
      ? "New Task"
      : location.pathname.includes("/edit")
        ? "Edit Task"
        : location.pathname.includes("/tasks/")
          ? "Task Details"
          : "TaskFlow");

  return (
    <div className="app-shell">
      <Sidebar open={menuOpen} onClose={() => setMenuOpen(false)} />
      <div className="main-area">
        <Header
          title={title}
          user={user || {}}
          onMenuToggle={() => setMenuOpen(true)}
        />
        <main className="content">{children}</main>
      </div>
    </div>
  );
}
