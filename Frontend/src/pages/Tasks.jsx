import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import toast from "react-hot-toast";
import API from "../services/api";
import DeleteModal from "../components/DeleteModal";
import { Badge } from "./Dashboard";

export default function Tasks() {
  const [tasks, setTasks] = useState([]);
  const [search, setSearch] = useState("");
  const [status, setStatus] = useState("");
  const [priority, setPriority] = useState("");
  const [deleteTask, setDeleteTask] = useState(null);

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
          <p>Manage and track your tasks.</p>
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

      <section className="card">
        {filtered.length === 0 ? (
          <div className="empty">No matching tasks found.</div>
        ) : (
          <div className="table-wrap">
            <table>
              <thead>
                <tr>
                  <th>Task</th>
                  <th>Category</th>
                  <th>Priority</th>
                  <th>Status</th>
                  <th>Due Date</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {filtered.map((task) => (
                  <tr key={task.id}>
                    <td>
                      <Link className="task-title" to={`/tasks/${task.id}`}>
                        {task.title}
                      </Link>
                      <small>{task.description || "No description"}</small>
                    </td>
                    <td>{task.categoryName || "-"}</td>
                    <td>
                      <Badge type="priority" value={task.priority} />
                    </td>
                    <td>
                      <Badge type="status" value={task.status} />
                    </td>
                    <td>{formatDate(task.dueDate)}</td>
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
        )}
      </section>

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
  return new Date(date).toLocaleDateString();
}
