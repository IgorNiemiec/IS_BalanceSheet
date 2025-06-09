import { useForm } from "react-hook-form";
import { useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";

import "../../styles/LoginForm.css"

type FormData = {
  username: string;
  password: string;
};

export function LoginForm() {
  const {
    register,
    handleSubmit,
    formState: { isSubmitting },
  } = useForm<FormData>();
  const [error, setError] = useState<string | null>(null);
  const { login } = useAuth();
  const navigate = useNavigate();

  const onSubmit = async (data: FormData) => {
    setError(null);
    try {
      const response = await fetch("http://localhost:5244/api/auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      if (!response.ok) {
        throw new Error("Błędny login lub hasło.");
      }

      const json = await response.json();
      login(json.token); 
      navigate("/dashboard");
    } catch (err: any) {
      setError(err.message || "Wystąpił błąd.");
    }
  };

  return (
     <form onSubmit={handleSubmit(onSubmit)} className="login-form">
      <div className="form-group">
        <label htmlFor="username">Nazwa użytkownika</label>
        <input id="username" {...register("username")} required />
      </div>
      <div className="form-group">
        <label htmlFor="password">Hasło</label>
        <input id="password" type="password" {...register("password")} required />
      </div>
      {error && <p className="form-error">{error}</p>}
      <button type="submit" className="form-button" disabled={isSubmitting}>
        {isSubmitting ? <span className="loader"></span> : "Zaloguj się"}
      </button>
    </form>
  );
}
