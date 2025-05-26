import RegisterForm from "../components/register/RegisterForm";
import "../styles/RegisterPageStyle.css";

export default function RegisterPage() {
  return (
    <div className="register-page">
      <h1>Rejestracja</h1>
      <RegisterForm />
    </div>
  );
}
