import axios from "axios";

const API = axios.create({
  baseURL: import.meta.env.VITE_API_URL || "http://localhost:5135/api",
});

export const getUserFromToken = () => {
  const token = localStorage.getItem("token");

  if (!token) return null;

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));

    return {
      id: payload.sub,
      email: payload.email,
      role:
        payload[
          "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        ] || payload.role,
      isAdmin:
        (payload[
          "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        ] || payload.role) === "Admin",
    };
  } catch {
    return null;
  }
};

API.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

API.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem("token");
      window.location.href = "/login";
    }

    return Promise.reject(error);
  },
);

export default API;
