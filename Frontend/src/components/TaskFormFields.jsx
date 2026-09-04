export default function TaskFormFields({ form, setForm, categories, isEdit }) {
  const change = (key, value) => setForm({ ...form, [key]: value });

  return (
    <>
      <div className="form-group">
        <label>Title *</label>
        <input
          value={form.title}
          onChange={(e) => change("title", e.target.value)}
          placeholder="Enter task title"
          required
        />
      </div>

      <div className="form-group">
        <label>Description</label>
        <textarea
          value={form.description}
          onChange={(e) => change("description", e.target.value)}
          placeholder="Describe the task..."
          rows="4"
        />
      </div>

      <div className="form-grid">
        <div className="form-group">
          <label>Category *</label>
          <select
            value={form.categoryId}
            onChange={(e) => change("categoryId", e.target.value)}
            required
          >
            <option value="">Select category</option>
            {categories.map((c) => (
              <option key={c.id} value={c.id}>
                {c.name}
              </option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label>Priority *</label>
          <select
            value={form.priority}
            onChange={(e) => change("priority", Number(e.target.value))}
          >
            <option value={1}>Low</option>
            <option value={2}>Medium</option>
            <option value={3}>High</option>
            <option value={4}>Critical</option>
          </select>
        </div>

        {isEdit && (
          <div className="form-group">
            <label>Status *</label>
            <select
              value={form.status}
              onChange={(e) => change("status", Number(e.target.value))}
            >
              <option value={1}>Pending</option>
              <option value={2}>In Progress</option>
              <option value={3}>Completed</option>
            </select>
          </div>
        )}

        <div className="form-group">
          <label>Due Date</label>
          <input
            type="datetime-local"
            value={form.dueDate}
            onChange={(e) => change("dueDate", e.target.value)}
          />
        </div>
      </div>
    </>
  );
}
