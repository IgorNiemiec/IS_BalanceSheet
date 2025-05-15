import { createBrowserRouter } from "react-router-dom"
import LoginPage from "../pages/LoginPage"
import DashboardPage from "../pages/DashboardPage"
import HomePage from "../pages/HomePage"
import { RequireAuth } from "../auth/RequireAuth"
import ProtectedRoute from "../auth/ProtectedRoute";
export const router = createBrowserRouter([
{
path: "/",
element: <HomePage />,
},
{
path: "/login",
element: <LoginPage />,
},
{
path: "/dashboard",
element: (
<RequireAuth>
      <ProtectedRoute>
      <DashboardPage />
    </ProtectedRoute>
</RequireAuth>
),

},
])