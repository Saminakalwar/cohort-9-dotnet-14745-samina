import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import API, { getUserFromToken } from "../services/api";

export default function Dashboard() {
  const [stats, setStats] = useState({
    pending: 0,
    inProgress: 0,
    completed: 0,
  });

  const [tasks, setTasks] = useState([]);

  const currentUser = getUserFromToken();
  const isAdmin = currentUser?.isAdmin;

  useEffect(() => {
    Promise.all([API.get("/Dashboard"), API.get("/Task")])
      .then(([dashboard, taskRes]) => {
        setStats(dashboard.data);
        setTasks(taskRes.data.slice(0, 5));
      })
      .catch(() => {});
  }, []);

  const formatDate = (date) => {
    if (!date) return "—";

    return new Date(date).toLocaleDateString("en-US", {
      month: "short",
      day: "numeric",
      year: "numeric",
    });
  };

  return (
    <div className="p-4 sm:p-6 max-w-6xl space-y-6">
      {/* Page heading */}
      <div className="flex items-center justify-between">
        <div>
          <h2
            className="text-2xl font-bold text-[#0D0F14]"
            style={{ fontFamily: "Outfit, sans-serif" }}
          >
            Dashboard
          </h2>

          <p className="text-sm text-[#6B7280] mt-0.5">
            {isAdmin
              ? "Overview of all tasks and activity"
              : "Overview of your tasks and activity"}
          </p>
        </div>

        <Link
          className="flex items-center gap-2 px-4 py-2.5 bg-[#4F46E5] text-white text-sm font-semibold rounded-lg hover:bg-[#4338CA] transition-colors"
          to="/tasks/new"
          style={{ fontFamily: "Outfit, sans-serif" }}
        >
          <span className="text-lg leading-none">+</span>

          <span className="hidden sm:inline">New Task</span>
          <span className="sm:hidden">New</span>
        </Link>
      </div>

      {/* Stats */}
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
        <Stat
          title="Pending Tasks"
          value={stats.pending}
          icon="◷"
          iconClass="bg-amber-50 text-amber-600"
        />

        <Stat
          title="In Progress"
          value={stats.inProgress}
          icon="↗"
          iconClass="bg-blue-50 text-blue-600"
        />

        <Stat
          title="Completed"
          value={stats.completed}
          icon="✓"
          iconClass="bg-emerald-50 text-emerald-600"
        />
      </div>

      {/* Recent Tasks */}
      <section className="bg-white rounded-xl border border-[#E4E7F0] overflow-hidden">
        {/* Section header */}
        <div className="px-5 py-4 border-b border-[#E4E7F0] flex items-center justify-between">
          <div>
            <h3
              className="text-base font-semibold text-[#0D0F14]"
              style={{ fontFamily: "Outfit, sans-serif" }}
            >
              {isAdmin ? "Recent Tasks" : "Your Recent Tasks"}
            </h3>

            <p className="text-sm text-[#6B7280] mt-0.5">
              {isAdmin ? "Latest tasks across all users" : "Your latest tasks"}
            </p>
          </div>

          <Link
            to="/tasks"
            className="text-sm text-[#4F46E5] hover:text-[#4338CA] font-medium transition-colors"
            style={{ fontFamily: "Outfit, sans-serif" }}
          >
            {isAdmin ? "View all tasks →" : "View your tasks →"}
          </Link>
        </div>

        {tasks.length === 0 ? (
          <div className="p-12 text-center text-sm text-[#6B7280]">
            No tasks yet. Create your first task.
          </div>
        ) : (
          <>
            {/* Desktop table */}
            <div className="hidden sm:block overflow-x-auto">
              <table className="w-full text-sm">
                <thead>
                  <tr className="border-b border-[#E4E7F0] bg-[#F5F7FB]">
                    <th className="text-left px-5 py-3 text-xs font-semibold text-[#6B7280] uppercase tracking-wide">
                      Title
                    </th>

                    {isAdmin && (
                      <th className="text-left px-5 py-3 text-xs font-semibold text-[#6B7280] uppercase tracking-wide">
                        Assigned To
                      </th>
                    )}

                    <th className="text-left px-5 py-3 text-xs font-semibold text-[#6B7280] uppercase tracking-wide">
                      Category
                    </th>

                    <th className="text-left px-5 py-3 text-xs font-semibold text-[#6B7280] uppercase tracking-wide">
                      Priority
                    </th>

                    <th className="text-left px-5 py-3 text-xs font-semibold text-[#6B7280] uppercase tracking-wide">
                      Status
                    </th>

                    <th className="text-left px-5 py-3 text-xs font-semibold text-[#6B7280] uppercase tracking-wide">
                      Due Date
                    </th>
                  </tr>
                </thead>

                <tbody className="divide-y divide-[#E4E7F0]">
                  {tasks.map((task) => (
                    <tr
                      key={task.id}
                      className="hover:bg-[#F5F7FB] cursor-pointer transition-colors"
                    >
                      {/* Title */}
                      <td className="px-5 py-3.5">
                        <Link to={`/tasks/${task.id}`} className="block">
                          <div className="font-medium text-[#0D0F14] hover:text-[#4F46E5] transition-colors">
                            {task.title}
                          </div>

                          {task.description && (
                            <div className="text-xs text-[#9CA3AF] mt-0.5 truncate max-w-[220px]">
                              {task.description}
                            </div>
                          )}
                        </Link>
                      </td>

                      {/* Admin only: Assignee */}
                      {isAdmin && (
                        <td className="px-5 py-3.5">
                          <div className="flex flex-col">
                            <span className="font-medium text-[#374151]">
                              {task.assignedUserName || "Unknown user"}
                            </span>

                            {task.assignedUserEmail && (
                              <span className="text-xs text-[#9CA3AF] mt-0.5">
                                {task.assignedUserEmail}
                              </span>
                            )}
                          </div>
                        </td>
                      )}

                      {/* Category */}
                      <td className="px-5 py-3.5">
                        <span className="text-[#6B7280]">
                          {task.categoryName || "No category"}
                        </span>
                      </td>

                      {/* Priority */}
                      <td className="px-5 py-3.5">
                        <Badge type="priority" value={task.priority} />
                      </td>

                      {/* Status */}
                      <td className="px-5 py-3.5">
                        <Badge type="status" value={task.status} />
                      </td>

                      {/* Due date */}
                      <td className="px-5 py-3.5 text-[#6B7280] whitespace-nowrap">
                        {formatDate(task.dueDate)}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            {/* Mobile cards */}
            <div className="sm:hidden divide-y divide-[#E4E7F0]">
              {tasks.map((task) => (
                <Link
                  key={task.id}
                  to={`/tasks/${task.id}`}
                  className="block p-4 hover:bg-[#F5F7FB] transition-colors"
                >
                  <div className="flex items-start justify-between gap-2 mb-2">
                    <div>
                      <div className="font-medium text-[#0D0F14] text-sm">
                        {task.title}
                      </div>

                      {task.description && (
                        <div className="text-xs text-[#9CA3AF] mt-0.5 line-clamp-2">
                          {task.description}
                        </div>
                      )}
                    </div>

                    <Badge type="status" value={task.status} />
                  </div>

                  <div className="flex items-center gap-2 flex-wrap">
                    <span className="text-xs text-[#6B7280]">
                      {task.categoryName || "No category"}
                    </span>

                    <span className="text-[#D1D5DB]">·</span>

                    <Badge type="priority" value={task.priority} />

                    <span className="text-[#D1D5DB]">·</span>

                    <span className="text-xs text-[#6B7280]">
                      {formatDate(task.dueDate)}
                    </span>
                  </div>

                  {isAdmin && (
                    <div className="mt-3 pt-3 border-t border-[#E4E7F0]">
                      <div className="text-xs text-[#6B7280]">Assigned to</div>

                      <div className="text-sm font-medium text-[#374151]">
                        {task.assignedUserName || "Unknown user"}
                      </div>

                      {task.assignedUserEmail && (
                        <div className="text-xs text-[#9CA3AF]">
                          {task.assignedUserEmail}
                        </div>
                      )}
                    </div>
                  )}
                </Link>
              ))}
            </div>
          </>
        )}
      </section>
    </div>
  );
}

function Stat({ title, value, icon, iconClass }) {
  return (
    <div className="bg-white rounded-xl border border-[#E4E7F0] p-5 flex items-center gap-4">
      <div
        className={`w-12 h-12 rounded-xl flex items-center justify-center flex-shrink-0 text-xl ${iconClass}`}
      >
        {icon}
      </div>

      <div>
        <div
          className="text-2xl font-bold text-[#0D0F14]"
          style={{ fontFamily: "Outfit, sans-serif" }}
        >
          {value}
        </div>

        <div className="text-sm text-[#6B7280] mt-0.5">{title}</div>
      </div>
    </div>
  );
}

export function Badge({ type, value }) {
  const priority = {
    1: "Low",
    2: "Medium",
    3: "High",
    4: "Critical",
  };

  const status = {
    1: "Pending",
    2: "In Progress",
    3: "Completed",
  };

  const label = type === "priority" ? priority[value] : status[value];

  return <span className={`badge ${type}-${value}`}>{label || "Unknown"}</span>;
}
