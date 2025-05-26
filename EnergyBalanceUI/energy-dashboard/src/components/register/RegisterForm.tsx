import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

import "../../styles/RegisterPageStyle.css"

export default function RegisterForm() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const validateForm = () => {
    if (!username.trim() || !password.trim()) {
      setError("Wszystkie pola są wymagane.");
      return false;
    }
    if (password.length < 6) {
      setError("Hasło musi mieć co najmniej 6 znaków.");
      return false;
    }
    setError(null);
    return true;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validateForm()) return;

    setLoading(true);
    try {
      const response = await axios.post("http://localhost:5244/api/auth/register", {
        nazwaUzytkownika: username,
        haslo: password,
      });

      if (response.status === 200 || response.status === 201) {
        // Po udanej rejestracji przekieruj do logowania
        navigate("/login");
      } else {
        setError("Rejestracja nie powiodła się.");
      }
    } catch (err: any) {
      if (err.response?.data?.message) {
        setError(err.response.data.message);
      } else {
        setError("Wystąpił błąd podczas rejestracji.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <form className="register-form" onSubmit={handleSubmit} noValidate>
      {error && <div className="error-message">{error}</div>}

      <label htmlFor="username">Nazwa użytkownika</label>
      <input
        id="username"
        type="text"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        autoComplete="username"
        required
        disabled={loading}
      />

      <label htmlFor="password">Hasło</label>
      <input
        id="password"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        autoComplete="new-password"
        required
        minLength={6}
        disabled={loading}
      />

      <button type="submit" disabled={loading}>
        {loading ? "Rejestruję..." : "Zarejestruj się"}
      </button>
    </form>
  );
}
