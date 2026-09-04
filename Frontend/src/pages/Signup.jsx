import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import API from "../services/api";

export default function Signup() {
  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
  });

  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const change = (key, value) => {
    setForm({ ...form, [key]: value });
  };

  const submit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      // Create account
      await API.post("/Auth/register", form);

      toast.success("Account created successfully.");

      // Automatically login
      const loginResponse = await API.post("/Auth/login", {
        email: form.email,
        password: form.password,
      });

      // Save JWT
      localStorage.setItem("token", loginResponse.data.token);

      // Go directly to dashboard
      navigate("/dashboard");
    } catch (err) {
      toast.error(err.response?.data?.message || "Registration failed.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-brand">
        <span className="brand-icon">✓</span>
        <b>TaskFlow</b>
      </div>

      <div className="auth-card">
        <h1>Create your account</h1>

        <p className="auth-subtitle">Start organizing your work today.</p>

        <form onSubmit={submit}>
          <div className="form-grid">
            <div className="form-group">
              <label>First Name</label>

              <input
                value={form.firstName}
                onChange={(e) => change("firstName", e.target.value)}
                required
              />
            </div>

            <div className="form-group">
              <label>Last Name</label>

              <input
                value={form.lastName}
                onChange={(e) => change("lastName", e.target.value)}
                required
              />
            </div>
          </div>

          <div className="form-group">
            <label>Email</label>

            <input
              type="email"
              value={form.email}
              onChange={(e) => change("email", e.target.value)}
              required
            />
          </div>

          <div className="form-group">
            <label>Password</label>

            <input
              type="password"
              value={form.password}
              onChange={(e) => change("password", e.target.value)}
              required
              minLength="6"
            />
          </div>

          <button className="auth-button" disabled={loading}>
            {loading ? "Creating..." : "Create Account"}
          </button>
        </form>

        <p className="auth-footer">
          Already have an account? <Link to="/login">Login</Link>
        </p>
      </div>
    </div>
  );
}
