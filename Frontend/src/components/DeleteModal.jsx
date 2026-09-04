export default function DeleteModal({ taskTitle, onConfirm, onCancel }) {
  return (
    <div className="modal-wrap">
      <div className="modal-backdrop" onClick={onCancel} />
      <div className="modal">
        <div className="modal-row">
          <div className="danger-icon">⌫</div>
          <div>
            <h3>Delete Task</h3>
            <p>
              Are you sure you want to delete <strong>"{taskTitle}"</strong>?
              This action cannot be undone.
            </p>
          </div>
        </div>
        <div className="modal-actions">
          <button className="btn btn-secondary" onClick={onCancel}>
            Cancel
          </button>
          <button className="btn btn-danger" onClick={onConfirm}>
            Delete Task
          </button>
        </div>
      </div>
    </div>
  );
}
