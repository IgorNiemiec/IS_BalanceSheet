import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import LoginPage from "./pages/LoginPage";
import { Card, CardContent } from "./components/ui/card";
import { Input} from "./components/ui/input"
import RegisterPage from "./pages/RegisterPage";

function App() {
return (
<AuthProvider>
<Router>
<Routes>
<Route
path="/"
element={
<div className="min-h-screen flex items-center justify-center bg-gray-100">
  
<Input placeholder="Testowe pole" />
<Card className="w-full max-w-md shadow-xl">
<CardContent className="p-6 space-y-4">
<h1 className="text-2xl font-bold text-center text-blue-600">
Energy Dashboard ⚡
</h1>
</CardContent>
</Card>
</div>
}
/>
<Route path="/login" element={<LoginPage />} />
<Route path="/register" element={<RegisterPage/>} />
</Routes>
</Router>
</AuthProvider>




);
}

export default App;