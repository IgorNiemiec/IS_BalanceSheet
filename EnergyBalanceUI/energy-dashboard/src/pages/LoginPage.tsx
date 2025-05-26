import { LoginForm } from "../components/login/LoginForm"

import "../styles/LoginPage.css"

export default function LoginPage() {
return (
<div className="login-container">
      <div className="login-card">
        <h2 className="login-title">Logowanie do systemu</h2>
        <LoginForm />
      </div>
    </div>
)
}