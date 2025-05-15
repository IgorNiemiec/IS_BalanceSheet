import { useForm } from "react-hook-form";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import { Loader } from "../ui/loader";
import { useState } from "react";
import { useAuth } from "../../context/AuthContext";
import { useNavigate } from "react-router-dom";

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
      login(json.token); // Zapisujemy token
      navigate("/dashboard"); // Przekierowanie po zalogowaniu
    } catch (err: any) {
      setError(err.message || "Wystąpił błąd.");
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
      <div className="space-y-2">
        <Label htmlFor="username">Nazwa użytkownika</Label>
        <Input id="username" {...register("username")} required />
      </div>
      <div className="space-y-2">
        <Label htmlFor="password">Hasło</Label>
        <Input id="password" type="password" {...register("password")} required />
      </div>
      {error && <p className="text-red-500 text-sm">{error}</p>}
      <Button type="submit" disabled={isSubmitting}>
        {isSubmitting ? <Loader className="mr-2" /> : null}
        Zaloguj się
      </Button>
    </form>
  );
}
