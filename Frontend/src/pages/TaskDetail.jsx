import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import toast from "react-hot-toast";
import API from "../services/api";
import DeleteModal from "../components/DeleteModal";
import { Badge } from "./Dashboard";

export default function TaskDetail() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [task, setTask] = useState(null);
  const [deleteOpen, setDeleteOpen] = useState(false);

  useEffect(() => {
    API.get(`/Task/${id}`)
      .then((res) => setTask(res.data))
      .catch(() => toast.error("Task not found."));
  }, [id]);

  const remove = async () => {
    try {
      await API.delete(`/Task/${id}`);
      toast.success("Task deleted.");
      navigate("/tasks");
    } catch (err) {
      toast.error(err.response?.data?.message || "Delete failed.");
    }
  };

  if (!task) return <div className="loading">Loading task...</div>;

  return (
    <>
      <div className="page-heading">
        <div>
          <Link className="back-link" to="/tasks">
            ← Back to Tasks
          </Link>
          <h2>{task.title}</h2>
        </div>
        <div className="heading-actions">
          <Link className="btn btn-secondary" to={`/tasks/${id}/edit`}>
            Edit
          </Link>
          <button
            className="btn btn-danger"
            onClick={() => setDeleteOpen(true)}
          >
            Delete
          </button>
        </div>
      </div>

      <section className="detail-card card">
        <div className="detail-description">
          <label>Description</label>
          <p>{task.description || "No description provided."}</p>
        </div>
        <div className="detail-grid">
          <Detail label="Category" value={task.categoryName || "-"} />
          <Detail label="Priority">
            <Badge type="priority" value={task.priority} />
          </Detail>
          <Detail label="Status">
            <Badge type="status" value={task.status} />
          </Detail>
          <Detail
            label="Due Date"
            value={task.dueDate ? new Date(task.dueDate).toLocaleString() : "-"}
          />
          <Detail label="Assigned User" value={task.assignedUserId || "-"} />
        </div>
      </section>

      {deleteOpen && (
        <DeleteModal
          taskTitle={task.title}
          onConfirm={remove}
          onCancel={() => setDeleteOpen(false)}
        />
      )}
    </>
  );
}

function Detail({ label, value, children }) {
  return (
    <div>
      <label>{label}</label>
      <div className="detail-value">{children || value}</div>
    </div>
  );
}
