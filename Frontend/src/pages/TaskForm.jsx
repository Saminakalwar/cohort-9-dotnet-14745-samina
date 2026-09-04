import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import toast from "react-hot-toast";
import API from "../services/api";
import TaskFormFields from "../components/TaskFormFields";

const initial = {
  title: "",
  description: "",
  dueDate: "",
  priority: 1,
  status: 1,
  categoryId: "",
};

export default function TaskForm() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();
  const [form, setForm] = useState(initial);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    API.get("/Categories")
      .then((res) => setCategories(res.data))
      .catch(() => toast.error("Could not load categories."));
    if (isEdit) {
      API.get(`/Task/${id}`)
        .then((res) => {
          const t = res.data;
          setForm({
            title: t.title || "",
            description: t.description || "",
            dueDate: toInputDate(t.dueDate),
            priority: t.priority,
            status: t.status,
            categoryId: t.categoryId,
          });
        })
        .catch(() => toast.error("Could not load task."));
    }
  }, [id, isEdit]);

  const submit = async (e) => {
    e.preventDefault();
    if (!form.categoryId) return toast.error("Please select a category.");
    setLoading(true);

    const payload = {
      title: form.title,
      description: form.description || null,
      dueDate: form.dueDate || null,
      priority: Number(form.priority),
      categoryId: form.categoryId,
      ...(isEdit ? { status: Number(form.status) } : {}),
    };

    try {
      if (isEdit) {
        await API.put(`/Task/${id}`, payload);
        toast.success("Task updated.");
        navigate(`/tasks/${id}`);
      } else {
        await API.post("/Task", payload);
        toast.success("Task created.");
        navigate("/tasks");
      }
    } catch (err) {
      toast.error(err.response?.data?.message || "Could not save task.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <section className="form-card card">
      <div className="form-card-heading">
        <h2>{isEdit ? "Edit Task" : "Create New Task"}</h2>
        <p>
          {isEdit
            ? "Update task information."
            : "Add a new task to your workspace."}
        </p>
      </div>
      <form onSubmit={submit}>
        <TaskFormFields
          form={form}
          setForm={setForm}
          categories={categories}
          isEdit={isEdit}
        />
        <div className="form-actions">
          <button
            type="button"
            className="btn btn-secondary"
            onClick={() => navigate(-1)}
          >
            Cancel
          </button>
          <button className="btn btn-primary" disabled={loading}>
            {loading ? "Saving..." : isEdit ? "Save Changes" : "Create Task"}
          </button>
        </div>
      </form>
    </section>
  );
}

function toInputDate(date) {
  if (!date) return "";
  const d = new Date(date);
  const pad = (n) => String(n).padStart(2, "0");
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;
}
