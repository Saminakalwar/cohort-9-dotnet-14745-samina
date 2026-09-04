import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import toast from "react-hot-toast";
import API from "../services/api";
import DeleteModal from "../components/DeleteModal";
import { Badge } from "./Dashboard";
import { getUserFromToken } from "../services/api";

export default function Tasks() {
  const [tasks, setTasks] = useState([]);
  const [search, setSearch] = useState("");
  const [status, setStatus] = useState("");
  const [priority, setPriority] = useState("");
  const [deleteTask, setDeleteTask] = useState(null);

  const currentUser = getUserFromToken();
  const isAdmin = currentUser?.isAdmin;

  const loadTasks = () => API.get("/Task").then((res) => setTasks(res.data));

  useEffect(() => {
    loadTasks().catch(() => toast.error("Could not load tasks."));
  }, []);

  const filtered = tasks.filter(
    (t) =>
      t.title.toLowerCase().includes(search.toLowerCase()) &&
      (!status || String(t.status) === status) &&
      (!priority || String(t.priority) === priority),
  );

  const remove = async () => {
    try {
      await API.delete(`/Task/${deleteTask.id}`);
      setTasks(tasks.filter((t) => t.id !== deleteTask.id));
      toast.success("Task deleted.");
    } catch (err) {
      toast.error(err.response?.data?.message || "Delete failed.");
    } finally {
      setDeleteTask(null);
    }
  };

  return (
    <>
      <div className="page-heading">
        <div>
          <h2>Tasks</h2>
          <p>
            {isAdmin
              ? "Manage and track the tasks."
              : "Manage and track your tasks."}
          </p>
        </div>
        <Link className="btn btn-primary" to="/tasks/new">
          + New Task
        </Link>
      </div>

      <div className="filters card">
        <input
          placeholder="Search tasks..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <select value={status} onChange={(e) => setStatus(e.target.value)}>
          <option value="">All Statuses</option>
          <option value="1">Pending</option>
          <option value="2">In Progress</option>
          <option value="3">Completed</option>
        </select>
        <select value={priority} onChange={(e) => setPriority(e.target.value)}>
          <option value="">All Priorities</option>
          <option value="1">Low</option>
          <option value="2">Medium</option>
          <option value="3">High</option>
          <option value="4">Critical</option>
        </select>
      </div>

      {/* Tasks */}
      {filtered.length > 0 && (
        <section className="card tasks-table-card">
          {/* ================= DESKTOP TABLE ================= */}
          <div className="table-wrap desktop-table">
            <table>
              <thead>
                <tr>
                  <th>Title</th>

                  {isAdmin && <th>Assigned To</th>}

                  <th>Category</th>
                  <th>Priority</th>
                  <th>Status</th>
                  <th>Due Date</th>
                  <th className="actions-header">Actions</th>
                </tr>
              </thead>

              <tbody>
                {filtered.map((task) => (
                  <tr key={task.id}>
                    {/* Title */}
                    <td className="task-title-cell">
                      <Link className="task-title" to={`/tasks/${task.id}`}>
                        {task.title}
                      </Link>

                      <small>{task.description || "No description"}</small>
                    </td>

                    {/* Admin: Assigned User Name Only */}
                    {isAdmin && (
                      <td>
                        <div className="task-assignee">
                          <strong>
                            {task.assignedUserName || "Unknown user"}
                          </strong>
                        </div>
                      </td>
                    )}

                    {/* Category */}
                    <td>{task.categoryName || "-"}</td>

                    {/* Priority */}
                    <td>
                      <Badge type="priority" value={task.priority} />
                    </td>

                    {/* Status */}
                    <td>
                      <Badge type="status" value={task.status} />
                    </td>

                    {/* Due Date */}
                    <td className="due-date">{formatDate(task.dueDate)}</td>

                    {/* Actions */}
                    <td className="actions">
                      <Link to={`/tasks/${task.id}`}>View</Link>

                      <Link to={`/tasks/${task.id}/edit`}>Edit</Link>

                      <button onClick={() => setDeleteTask(task)}>
                        Delete
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {/* ================= MOBILE CARDS ================= */}
          <div className="mobile-task-list">
            {filtered.map((task) => (
              <div className="mobile-task-card" key={task.id}>
                {/* Title */}
                <div className="mobile-task-header">
                  <div>
                    <Link
                      className="mobile-task-title"
                      to={`/tasks/${task.id}`}
                    >
                      {task.title}
                    </Link>

                    <p>{task.description || "No description"}</p>
                  </div>
                </div>

                {/* Badges */}
                <div className="mobile-task-badges">
                  <Badge type="status" value={task.status} />

                  <Badge type="priority" value={task.priority} />

                  <span className="category-badge">
                    {task.categoryName || "No category"}
                  </span>
                </div>

                {/* Admin: Assigned User Name Only */}
                {isAdmin && (
                  <div className="mobile-assignee">
                    <span>Assigned to</span>

                    <strong>{task.assignedUserName || "Unknown user"}</strong>
                  </div>
                )}

                {/* Bottom */}
                <div className="mobile-task-footer">
                  <span className="mobile-due-date">
                    Due {formatDate(task.dueDate)}
                  </span>

                  <div className="mobile-actions">
                    <Link to={`/tasks/${task.id}`} title="View">
                      👁
                    </Link>

                    <Link to={`/tasks/${task.id}/edit`} title="Edit">
                      ✎
                    </Link>

                    <button onClick={() => setDeleteTask(task)} title="Delete">
                      🗑
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </section>
      )}

      {/* Delete Modal */}
      {deleteTask && (
        <DeleteModal
          taskTitle={deleteTask.title}
          onConfirm={remove}
          onCancel={() => setDeleteTask(null)}
        />
      )}
    </>
  );
}

function formatDate(date) {
  if (!date) return "-";

  return new Date(date).toLocaleDateString("en-US", {
    month: "short",
    day: "numeric",
    year: "numeric",
  });
}
