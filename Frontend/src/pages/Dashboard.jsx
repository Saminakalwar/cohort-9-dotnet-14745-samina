import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import API from "../services/api";

export default function Dashboard() {
  const [stats, setStats] = useState({
    pending: 0,
    inProgress: 0,
    completed: 0,
  });
  const [tasks, setTasks] = useState([]);

  useEffect(() => {
    Promise.all([API.get("/Dashboard"), API.get("/Task")])
      .then(([dashboard, taskRes]) => {
        setStats(dashboard.data);
        setTasks(taskRes.data.slice(0, 5));
      })
      .catch(() => {});
  }, []);

  return (
    <>
      <div className="page-heading">
        <div>
          <h2>Dashboard</h2>
          <p>Here's an overview of your work.</p>
        </div>
        <Link className="btn btn-primary" to="/tasks/new">
          + New Task
        </Link>
      </div>

      <div className="stats-grid">
        <Stat title="Pending Tasks" value={stats.pending} icon="◷" />
        <Stat title="In Progress" value={stats.inProgress} icon="↗" />
        <Stat title="Completed" value={stats.completed} icon="✓" />
      </div>

      <section className="card">
        <div className="section-header">
          <div>
            <h3>Recent Tasks</h3>
            <p>Your latest tasks</p>
          </div>
          <Link to="/tasks" className="text-link">
            View all
          </Link>
        </div>

        {tasks.length === 0 ? (
          <div className="empty">No tasks yet. Create your first task.</div>
        ) : (
          <div className="task-list-mini">
            {tasks.map((task) => (
              <Link to={`/tasks/${task.id}`} className="task-row" key={task.id}>
                <div>
                  <strong>{task.title}</strong>
                  <span>{task.categoryName || "No category"}</span>
                </div>
                <div className="task-row-right">
                  <Badge type="priority" value={task.priority} />
                  <Badge type="status" value={task.status} />
                </div>
              </Link>
            ))}
          </div>
        )}
      </section>
    </>
  );
}

function Stat({ title, value, icon }) {
  return (
    <div className="stat-card">
      <div className="stat-icon">{icon}</div>
      <div>
        <p>{title}</p>
        <strong>{value}</strong>
      </div>
    </div>
  );
}

export function Badge({ type, value }) {
  const priority = { 1: "Low", 2: "Medium", 3: "High", 4: "Critical" };
  const status = { 1: "Pending", 2: "In Progress", 3: "Completed" };
  const label = type === "priority" ? priority[value] : status[value];
  return <span className={`badge ${type}-${value}`}>{label || "Unknown"}</span>;
}
