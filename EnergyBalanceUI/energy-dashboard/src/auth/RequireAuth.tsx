import { useAuth } from "../context/AuthContext"
import { Navigate, useLocation } from "react-router-dom"
import { ReactNode } from "react"

export function RequireAuth({ children }: { children: ReactNode }) {
const { token } = useAuth()
const location = useLocation()

if (!token) {
// Przekierowanie na login, zapamiętując lokalizację
return <Navigate to="/login" state={{ from: location }} replace />
}

return <>{children}</>
}