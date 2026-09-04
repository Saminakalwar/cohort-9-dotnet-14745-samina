import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import API from "../services/api";

export default function Login() {
  const [form, setForm] = useState({
    email: "",
    password: "",
  });

  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  const validate = () => {
    const e = {};

    if (!form.email) {
      e.email = "Email is required";
    } else if (!/\S+@\S+\.\S+/.test(form.email)) {
      e.email = "Enter a valid email";
    }

    if (!form.password) {
      e.password = "Password is required";
    }

    setErrors(e);

    return Object.keys(e).length === 0;
  };

  const submit = async (e) => {
    e.preventDefault();

    if (!validate()) return;

    setLoading(true);

    try {
      const response = await API.post("/Auth/login", {
        email: form.email,
        password: form.password,
      });

      localStorage.setItem("token", response.data.token);

      toast.success("Welcome back!");

      navigate("/dashboard");
    } catch (error) {
      toast.error(
        error.response?.data?.message || "Invalid email or password.",
      );
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (field, value) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));

    // Remove the error for this field as the user starts typing
    setErrors((prev) => ({
      ...prev,
      [field]: undefined,
    }));
  };

  return (
    <AuthLayout title="Welcome back" subtitle="Sign in to manage your tasks.">
      <form onSubmit={submit} noValidate>
        {/* Email */}
        <div className="form-group">
          <label>Email</label>

          <input
            type="email"
            value={form.email}
            onChange={(e) => handleChange("email", e.target.value)}
            placeholder="you@example.com"
          />

          {errors.email && <p className="error-message">{errors.email}</p>}
        </div>

        {/* Password */}
        <div className="form-group">
          <label>Password</label>

          <input
            type="password"
            value={form.password}
            onChange={(e) => handleChange("password", e.target.value)}
            placeholder="••••••••"
          />

          {errors.password && (
            <p className="error-message">{errors.password}</p>
          )}
        </div>

        {/* Submit */}
        <button type="submit" className="auth-button" disabled={loading}>
          {loading ? "Signing in..." : "Login"}
        </button>
      </form>

      {/* Signup */}
      <p className="auth-footer">
        Don't have an account? <Link to="/signup">Create account</Link>
      </p>
    </AuthLayout>
  );
}

function AuthLayout({ title, subtitle, children }) {
  return (
    <div className="auth-page">
      <div className="auth-brand">
        <span className="brand-icon">✓</span>
        <b>TaskFlow</b>
      </div>

      <div className="auth-card">
        <h1>{title}</h1>
        <p className="auth-subtitle">{subtitle}</p>

        {children}
      </div>
    </div>
  );
}
