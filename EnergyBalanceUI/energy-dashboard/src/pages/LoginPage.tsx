import { LoginForm } from "../components/login/LoginForm"

export default function LoginPage() {
return (
<div className="min-h-screen flex items-center justify-center bg-gray-100">
<div className="w-full max-w-md bg-white p-6 rounded-xl shadow-lg">
<h2 className="text-2xl font-bold text-gray-800 mb-4 text-center">
Logowanie do systemu
</h2>
<LoginForm />
</div>
</div>
)
}